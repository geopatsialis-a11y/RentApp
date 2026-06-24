using System.Text.Json;
using API.DTOs.Asset;
using API.Entities;
using API.Errors;
using API.Helper;
using Microsoft.EntityFrameworkCore;
using static API.Entities.Enums;

namespace API.Data.Repositories;


public partial class AssetRepository
{
    // ==================================================================
    //  ASSET (data) — basic CRUD, non-filtered listing
    // ==================================================================
 
    public async Task<PaginatedResult<AssetDto>> GetAllAsync(
        PagingParams pagingParams, Guid? assetTypeId, AssetStatus? status)
    {
        var query = context.Assets.AsNoTracking()
            .Include(a => a.AssetType)
            .Include(a => a.AttributeValues).ThenInclude(v => v.AssetTypeField)
            .AsQueryable();
 
        if (assetTypeId.HasValue)
            query = query.Where(a => a.AssetTypeId == assetTypeId);
 
        if (status.HasValue)
            query = query.Where(a => a.Status == status);
 
        if (!string.IsNullOrWhiteSpace(pagingParams.Search))
        {
            var term = pagingParams.Search.Trim().ToLower();
            query = query.Where(a => a.Name.ToLower().Contains(term));
        }
 
        var ordered = query.OrderByDescending(a => a.CreatedAt);
 
        // Materialize entities first (attribute flattening needs in-memory work),
        // then page manually since PaginationHelper expects IQueryable<TDto> —
        // here we project to DTO via AsEnumerable to call the local mapper.
        var countQuery = ordered;
        var totalCount = await countQuery.CountAsync();
 
        var pageEntities = await ordered
            .Skip((pagingParams.Page - 1) * pagingParams.PageSize)
            .Take(pagingParams.PageSize)
            .ToListAsync();
 
        var items = pageEntities.Select(MapToDto).ToList();
 
        return new PaginatedResult<AssetDto>
        {
            Items = items,
            Metadata = new PaginationMetadata
            {
                CurrentPage = pagingParams.Page,
                PageSize = pagingParams.PageSize,
                TotalCount = totalCount,
                TotalPages = (int)Math.Ceiling(totalCount / (double)pagingParams.PageSize)
            }
        };
    }
 
    public async Task<AssetDto?> GetByIdAsync(Guid id)
    {
        var asset = await context.Assets.AsNoTracking()
            .Include(a => a.AssetType)
            .Include(a => a.AttributeValues).ThenInclude(v => v.AssetTypeField)
            .FirstOrDefaultAsync(a => a.Id == id);
 
        return asset == null ? null : MapToDto(asset);
    }
 
    public async Task<List<AssetLookupDto>> GetLookupAsync(string? search, Guid? assetTypeId)
    {
        var query = context.Assets.AsNoTracking().AsQueryable();
 
        if (assetTypeId.HasValue)
            query = query.Where(a => a.AssetTypeId == assetTypeId);
 
        if (!string.IsNullOrWhiteSpace(search))
        {
            var term = search.Trim().ToLower();
            query = query.Where(a => a.Name.ToLower().Contains(term));
        }
 
        return await query
            .OrderBy(a => a.Name)
            .Take(20)
            .Select(a => new AssetLookupDto { Id = a.Id, Name = a.Name, Status = a.Status })
            .ToListAsync();
    }
 
    public async Task<Asset?> GetEntityByIdAsync(Guid id)
    {
        return await context.Assets
            .Include(a => a.AttributeValues)
            .FirstOrDefaultAsync(a => a.Id == id);
    }
 
    public async Task AddAsync(Asset asset)
    {
        await context.Assets.AddAsync(asset);
    }
 
    public void Update(Asset asset)
    {
        context.Entry(asset).State = EntityState.Modified;
    }
 
    public void SoftDelete(Asset asset)
    {
        asset.IsDeleted = true;
        asset.DeletedAt = DateTime.UtcNow;
        context.Entry(asset).State = EntityState.Modified;
    }
 
    // ==================================================================
    //  ATTRIBUTE VALUES (EAV write path)
    //  Writes BOTH the typed AssetAttributeValue rows (for indexable
    //  filtering) AND the denormalized PropertiesJson cache (for fast
    //  reads without joins) — kept in sync on every write.
    // ==================================================================
 
    public async Task ReplaceAttributeValuesAsync(
        Asset asset, Dictionary<string, object?> attributes, List<AssetTypeField> schema)
    {
        // Remove any existing values for fields no longer supplied — covers
        // both "field cleared" on update and the initial create (no-op then).
        var existing = await context.AssetAttributeValues
            .Where(v => v.AssetId == asset.Id)
            .ToListAsync();
        context.AssetAttributeValues.RemoveRange(existing);
 
        var jsonProperties = new Dictionary<string, object?>();
 
        foreach (var field in schema)
        {
            if (!attributes.TryGetValue(field.Name, out var rawValue) || rawValue is null)
            {
                if (field.IsRequired)
                    throw new BadRequestException($"Field '{field.Label}' is required.");
                continue;
            }
 
            var value = new AssetAttributeValue
            {
                TenantId = asset.TenantId,
                AssetId = asset.Id,
                AssetTypeFieldId = field.Id,
                CreatedBy = asset.CreatedBy
            };
 
            switch (field.DataType)
            {
                case FieldDataType.Text:
                    value.StringValue = rawValue.ToString();
                    jsonProperties[field.Name] = value.StringValue;
                    break;
 
                case FieldDataType.Number:
                    if (!decimal.TryParse(rawValue.ToString(), out var num))
                        throw new BadRequestException($"Field '{field.Label}' must be numeric.");
                    if (field.MinValue.HasValue && num < field.MinValue)
                        throw new BadRequestException($"Field '{field.Label}' must be >= {field.MinValue}.");
                    if (field.MaxValue.HasValue && num > field.MaxValue)
                        throw new BadRequestException($"Field '{field.Label}' must be <= {field.MaxValue}.");
                    value.DecimalValue = num;
                    jsonProperties[field.Name] = num;
                    break;
 
                case FieldDataType.Boolean:
                    if (!bool.TryParse(rawValue.ToString(), out var boolVal))
                        throw new BadRequestException($"Field '{field.Label}' must be true/false.");
                    value.BoolValue = boolVal;
                    jsonProperties[field.Name] = boolVal;
                    break;
 
                case FieldDataType.Date:
                case FieldDataType.DateTime:
                    if (!DateTime.TryParse(rawValue.ToString(), out var dateVal))
                        throw new BadRequestException($"Field '{field.Label}' must be a valid date.");
                    value.DateValue = dateVal;
                    jsonProperties[field.Name] = dateVal.ToString("O");
                    break;
            }
 
            await context.AssetAttributeValues.AddAsync(value);
        }
 
        asset.PropertiesJson = JsonSerializer.SerializeToDocument(jsonProperties);
        context.Entry(asset).Property(a => a.PropertiesJson).IsModified = true;
    }
 
    // ==================================================================
    //  MAINTENANCE HISTORY (CostAssetHist)
    // ==================================================================
 
    public async Task<List<CostAssetHist>> GetMaintenanceHistoryAsync(Guid assetId)
    {
        return await context.CostAssetHists
            .AsNoTracking()
            .Where(h => h.AssetId == assetId)
            .OrderByDescending(h => h.Date)
            .ToListAsync();
    }
 
    public async Task AddMaintenanceRecordAsync(CostAssetHist record)
    {
        await context.CostAssetHists.AddAsync(record);
    }
 
    // ==================================================================
    //  Mapping: Asset entity (+ loaded AttributeValues) -> AssetDto
    // ==================================================================
 
    private static AssetDto MapToDto(Asset asset)
    {
        var attributes = new Dictionary<string, object?>();
 
        foreach (var value in asset.AttributeValues)
        {
            var key = value.AssetTypeField.Name;
            object? resolved = value.AssetTypeField.DataType switch
            {
                FieldDataType.Text => value.StringValue,
                FieldDataType.Number => value.DecimalValue,
                FieldDataType.Boolean => value.BoolValue,
                FieldDataType.Date or FieldDataType.DateTime => value.DateValue,
                _ => null
            };
            attributes[key] = resolved;
        }
 
        return new AssetDto
        {
            Id = asset.Id,
            AssetTypeId = asset.AssetTypeId,
            AssetTypeName = asset.AssetType?.Name ?? string.Empty,
            Name = asset.Name,
            Notes = asset.Notes,
            AcquisitionType = asset.AcquisitionType,
            AcquisitionCost = asset.AcquisitionCost,
            MonthlyLeaseCost = asset.MonthlyLeaseCost,
            Status = asset.Status,
            CreatedAt = asset.CreatedAt,
            Attributes = attributes
        };
    }
}

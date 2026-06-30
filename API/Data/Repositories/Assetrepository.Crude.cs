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
            .Skip((pagingParams.PageNumber - 1) * pagingParams.PageSize)
            .Take(pagingParams.PageSize)
            .ToListAsync();
 
        var items = pageEntities.Select(MapToDto).ToList();
 
        return new PaginatedResult<AssetDto>
        {
            Items = items,
            Metadata = new PaginationMetadata
            {
                CurrentPage = pagingParams.PageNumber,
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
 
            jsonProperties[field.Name] = CoerceIntoAttributeValue(field, rawValue, value);
 
            await context.AssetAttributeValues.AddAsync(value);
        }
 
        asset.PropertiesJson = JsonSerializer.SerializeToDocument(jsonProperties);
        context.Entry(asset).Property(a => a.PropertiesJson).IsModified = true;
    }
 
    // ==================================================================
    //  SINGLE ATTRIBUTE PATCH — change one field's value on an asset
    //  (e.g. just "color") without touching the rest of its attributes.
    // ==================================================================
 
    public async Task SetSingleAttributeValueAsync(Asset asset, AssetTypeField field, object? rawValue)
    {
        var existing = await context.AssetAttributeValues
            .FirstOrDefaultAsync(v => v.AssetId == asset.Id && v.AssetTypeFieldId == field.Id);
 
        object? jsonValue = null;
 
        if (rawValue is null)
        {
            if (field.IsRequired)
                throw new BadRequestException($"Field '{field.Label}' is required and cannot be cleared.");
 
            if (existing != null)
                context.AssetAttributeValues.Remove(existing);
        }
        else
        {
            var value = existing ?? new AssetAttributeValue
            {
                TenantId = asset.TenantId,
                AssetId = asset.Id,
                AssetTypeFieldId = field.Id,
                CreatedBy = asset.CreatedBy
            };
 
            jsonValue = CoerceIntoAttributeValue(field, rawValue, value);
 
            if (existing == null)
                await context.AssetAttributeValues.AddAsync(value);
            else
                context.Entry(value).State = EntityState.Modified;
        }
 
        // Keep the PropertiesJson read-cache in sync: load every OTHER
        // attribute's current value once, then overlay the field we just
        // changed (or removed) — one query, no re-fetch of what we already
        // just wrote above.
        var otherValues = await context.AssetAttributeValues
            .Include(v => v.AssetTypeField)
            .Where(v => v.AssetId == asset.Id && v.AssetTypeFieldId != field.Id)
            .ToListAsync();
 
        var jsonProperties = otherValues.ToDictionary(
            v => v.AssetTypeField.Name,
            v => v.AssetTypeField.DataType switch
            {
                FieldDataType.Text => (object?)v.StringValue,
                FieldDataType.Number => v.DecimalValue,
                FieldDataType.Boolean => v.BoolValue,
                FieldDataType.Date or FieldDataType.DateTime => v.DateValue?.ToString("O"),
                _ => null
            });
 
        if (rawValue is not null)
            jsonProperties[field.Name] = jsonValue;
 
        asset.PropertiesJson = JsonSerializer.SerializeToDocument(jsonProperties);
        context.Entry(asset).Property(a => a.PropertiesJson).IsModified = true;
    }
 
    // Coerces a raw (string/number/bool/JSON-element) value into the right
    // typed column on an AssetAttributeValue based on the field's DataType,
    // validating Min/MaxValue along the way. Returns the value in the form
    // it should appear as in the PropertiesJson cache (so callers building
    // that cache don't need to know the per-DataType shape themselves).
    private static object? CoerceIntoAttributeValue(AssetTypeField field, object rawValue, AssetAttributeValue target)
    {
        switch (field.DataType)
        {
            case FieldDataType.Text:
                target.StringValue = rawValue.ToString();
                return target.StringValue;
 
            case FieldDataType.Number:
                if (!decimal.TryParse(rawValue.ToString(), out var num))
                    throw new BadRequestException($"Field '{field.Label}' must be numeric.");
                if (field.MinValue.HasValue && num < field.MinValue)
                    throw new BadRequestException($"Field '{field.Label}' must be >= {field.MinValue}.");
                if (field.MaxValue.HasValue && num > field.MaxValue)
                    throw new BadRequestException($"Field '{field.Label}' must be <= {field.MaxValue}.");
                target.DecimalValue = num;
                return num;
 
            case FieldDataType.Boolean:
                if (!bool.TryParse(rawValue.ToString(), out var boolVal))
                    throw new BadRequestException($"Field '{field.Label}' must be true/false.");
                target.BoolValue = boolVal;
                return boolVal;
 
            case FieldDataType.Date:
            case FieldDataType.DateTime:
                if (!DateTime.TryParse(
                        rawValue.ToString(),
                        System.Globalization.CultureInfo.InvariantCulture,
                        System.Globalization.DateTimeStyles.None,
                        out var dateVal))
                    throw new BadRequestException($"Field '{field.Label}' must be a valid date.");
 
                // Npgsql maps DateTime -> "timestamp with time zone" and refuses
                // to write any value whose Kind isn't explicitly Utc (Kind=Unspecified
                // is what DateTime.TryParse produces for a plain "2021-03-15" string,
                // and Npgsql treats that as ambiguous/unsafe rather than guessing).
                dateVal = dateVal.Kind switch
                {
                    DateTimeKind.Utc => dateVal,
                    DateTimeKind.Local => dateVal.ToUniversalTime(),
                    _ => DateTime.SpecifyKind(dateVal, DateTimeKind.Utc)
                };
 
                target.DateValue = dateVal;
                return dateVal.ToString("O");
 
            default:
                return null;
        }
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
using API.DTOs.Asset;
using API.Entities;
using API.Errors;
using API.Helper;
using static API.Entities.Enums;
 
namespace API.Services;
 public partial class AssetService
{
    // ==================================================================
    //  ASSET (data) CRUD
    // ==================================================================

    public Task<PaginatedResult<AssetDto>> GetAllAsync(PagingParams pagingParams, Guid? assetTypeId, AssetStatus? status)
        => unitOfWork.AssetRepository.GetAllAsync(pagingParams, assetTypeId, status);

    public Task<AssetDto?> GetByIdAsync(Guid id)
        => unitOfWork.AssetRepository.GetByIdAsync(id);

    public Task<List<AssetLookupDto>> GetLookupAsync(string? search, Guid? assetTypeId)
        => unitOfWork.AssetRepository.GetLookupAsync(search, assetTypeId);

    public async Task<AssetDto> CreateAsync(AssetCreateDto dto, string currentUserId)
    {
        var assetType = await unitOfWork.AssetRepository.GetAssetTypeEntityByIdAsync(dto.AssetTypeId)
            ?? throw new NotFoundException($"Asset category '{dto.AssetTypeId}' was not found.");

        var schema = assetType.Fields.ToList();

        var asset = new Asset
        {
            TenantId = tenantProvider.TenantId,
            AssetTypeId = dto.AssetTypeId,
            Name = dto.Name,
            Notes = dto.Notes,
            AcquisitionType = dto.AcquisitionType,
            AcquisitionCost = dto.AcquisitionCost,
            MonthlyLeaseCost = dto.MonthlyLeaseCost,
            Status = AssetStatus.Available,
            CreatedBy = currentUserId
        };

        await unitOfWork.AssetRepository.AddAsync(asset);

        // Attribute values reference AssetId as their FK — EF Core resolves
        // this automatically once both are added to the same tracked context,
        // because Complete() generates the Asset's INSERT before the
        // AssetAttributeValue INSERTs in the same transaction.
        await unitOfWork.AssetRepository.ReplaceAttributeValuesAsync(asset, dto.Attributes, schema);
        await unitOfWork.Complete();

        return (await unitOfWork.AssetRepository.GetByIdAsync(asset.Id))!;
    }

     public async Task<AssetDto> UpdateAttributeAsync(Guid id, AssetAttributeUpdateDto dto, string currentUserId)
    {
        var asset = await unitOfWork.AssetRepository.GetEntityByIdAsync(id)
            ?? throw new NotFoundException($"Asset '{id}' was not found.");
 
        var schema = await unitOfWork.AssetRepository.GetFieldsForTypeAsync(asset.AssetTypeId);
        var field = schema.FirstOrDefault(f => f.Name == dto.FieldName)
            ?? throw new BadRequestException(
                $"'{dto.FieldName}' is not a valid field for this asset's category.");
 
        await unitOfWork.AssetRepository.SetSingleAttributeValueAsync(asset, field, dto.Value);
 
        asset.UpdatedAt = DateTime.UtcNow;
        asset.UpdatedBy = currentUserId;
        unitOfWork.AssetRepository.Update(asset);
 
        await unitOfWork.Complete();
 
        return (await unitOfWork.AssetRepository.GetByIdAsync(id))!;
    }

    public async Task<AssetDto> UpdateAsync(Guid id, AssetUpdateDto dto, string currentUserId)
    {
        var asset = await unitOfWork.AssetRepository.GetEntityByIdAsync(id)
            ?? throw new NotFoundException($"Asset '{id}' was not found.");

        var schema = await unitOfWork.AssetRepository.GetFieldsForTypeAsync(asset.AssetTypeId);

        asset.Name = dto.Name;
        asset.Notes = dto.Notes;
        asset.AcquisitionType = dto.AcquisitionType;
        asset.AcquisitionCost = dto.AcquisitionCost;
        asset.MonthlyLeaseCost = dto.MonthlyLeaseCost;
        asset.UpdatedAt = DateTime.UtcNow;
        asset.UpdatedBy = currentUserId;

        unitOfWork.AssetRepository.Update(asset);
        await unitOfWork.AssetRepository.ReplaceAttributeValuesAsync(asset, dto.Attributes, schema);
        await unitOfWork.Complete();

        return (await unitOfWork.AssetRepository.GetByIdAsync(id))!;
    }

    public async Task<AssetDto> UpdateStatusAsync(Guid id, AssetStatusUpdateDto dto, string currentUserId)
    {
        var asset = await unitOfWork.AssetRepository.GetEntityByIdAsync(id)
            ?? throw new NotFoundException($"Asset '{id}' was not found.");

        asset.Status = dto.Status;
        asset.UpdatedAt = DateTime.UtcNow;
        asset.UpdatedBy = currentUserId;

        unitOfWork.AssetRepository.Update(asset);
        await unitOfWork.Complete();

        return (await unitOfWork.AssetRepository.GetByIdAsync(id))!;
    }

    public async Task DeleteAsync(Guid id, string currentUserId)
    {
        var asset = await unitOfWork.AssetRepository.GetEntityByIdAsync(id)
            ?? throw new NotFoundException($"Asset '{id}' was not found.");

        if (asset.Status == AssetStatus.Rented)
            throw new BadRequestException("Cannot delete an asset that is currently rented.");

        asset.DeletedBy = currentUserId;
        unitOfWork.AssetRepository.SoftDelete(asset);
        await unitOfWork.Complete();
    }

    // ==================================================================
    //  DYNAMIC FACET SEARCH
    //  This is the validation gate that makes AssetRepository.SearchAsync's
    //  raw-SQL field-name interpolation safe: every FieldName in the request
    //  must be an exact match for a real field belonging to THIS tenant's
    //  THIS AssetType — never arbitrary text from the client.
    // ==================================================================

    public async Task<PaginatedResult<AssetDto>> SearchAsync(AssetSearchRequest request)
    {
        var schema = await unitOfWork.AssetRepository.GetFieldsForTypeAsync(request.AssetTypeId);
        if (schema.Count == 0)
            throw new NotFoundException($"Asset category '{request.AssetTypeId}' was not found or has no fields.");

        var schemaByName = schema.ToDictionary(f => f.Name);

        foreach (var filter in request.Filters)
        {
            if (!schemaByName.TryGetValue(filter.FieldName, out var field))
                throw new BadRequestException(
                    $"'{filter.FieldName}' is not a valid filter field for this asset category.");

            // Range filters only make sense for Number/Date fields; reject a
            // mismatched combination early rather than silently no-op-ing it
            // in the SQL (e.g. casting a Text column to ::numeric would 500).
            var isRangeFilter = filter.MinValue.HasValue || filter.MaxValue.HasValue
                              || filter.MinDate.HasValue || filter.MaxDate.HasValue;

            if (isRangeFilter && field.DataType is not (FieldDataType.Number or FieldDataType.Date or FieldDataType.DateTime))
                throw new BadRequestException($"Field '{field.Label}' does not support range filtering.");
        }

        return await unitOfWork.AssetRepository.SearchAsync(request);
    }

    // ==================================================================
    //  MAINTENANCE HISTORY
    // ==================================================================

    public async Task<List<CostAssetHistDto>> GetMaintenanceHistoryAsync(Guid assetId)
    {
        var history = await unitOfWork.AssetRepository.GetMaintenanceHistoryAsync(assetId);
        return history.Select(h => new CostAssetHistDto
        {
            Id = h.Id,
            Date = h.Date,
            Description = h.Description,
            Cost = h.Cost,
            MaintainedBy = h.MaintainedBy
        }).ToList();
    }

    public async Task<CostAssetHistDto> AddMaintenanceRecordAsync(Guid assetId, CostAssetHistCreateDto dto, string currentUserId)
    {
        var asset = await unitOfWork.AssetRepository.GetEntityByIdAsync(assetId)
            ?? throw new NotFoundException($"Asset '{assetId}' was not found.");

        var record = new CostAssetHist
        {
            TenantId = asset.TenantId,
            AssetId = assetId,
            // Same Npgsql UTC requirement as AssetAttributeValue.DateValue —
            // System.Text.Json deserializes a plain "2026-06-01" into
            // Kind=Unspecified, which Npgsql rejects for timestamptz columns.
            Date = DateTime.SpecifyKind(dto.Date, DateTimeKind.Utc),
            Description = dto.Description,
            Cost = dto.Cost,
            MaintainedBy = dto.MaintainedBy,
            CreatedBy = currentUserId
        };

        await unitOfWork.AssetRepository.AddMaintenanceRecordAsync(record);
        await unitOfWork.Complete();

        return new CostAssetHistDto
        {
            Id = record.Id,
            Date = record.Date,
            Description = record.Description,
            Cost = record.Cost,
            MaintainedBy = record.MaintainedBy
        };
    }
}
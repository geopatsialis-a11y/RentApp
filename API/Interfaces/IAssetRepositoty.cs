using System;
using API.DTOs;
using API.DTOs.Asset;
using API.Entities;
using API.Helper;

namespace API.Interfaces;

public interface IAssetRepository

{
    // ---------------- AssetType (category) ----------------
    Task<List<AssetTypeDto>> GetAssetTypesAsync();
    Task<List<AssetTypeLookupDto>> GetAssetTypeLookupAsync();
    Task<AssetTypeDto?> GetAssetTypeByIdAsync(Guid id);
    Task<AssetType?> GetAssetTypeEntityByIdAsync(Guid id);
    Task<bool> AssetTypeNameExistsAsync(string name, Guid? excludingId = null);
    Task AddAssetTypeAsync(AssetType assetType);
    void UpdateAssetType(AssetType assetType);
    Task<int> CountAssetsOfTypeAsync(Guid assetTypeId);
    void RemoveAssetType(AssetType assetType);
 
    // ---------------- AssetTypeField (dynamic schema) ----------------
    Task<AssetTypeField?> GetFieldEntityByIdAsync(Guid fieldId);
    Task<List<AssetTypeField>> GetFieldsForTypeAsync(Guid assetTypeId);
    Task<bool> FieldNameExistsAsync(Guid assetTypeId, string name, Guid? excludingId = null);
    Task AddFieldAsync(AssetTypeField field);
    void UpdateField(AssetTypeField field);
    void RemoveField(AssetTypeField field);
    Task<bool> FieldHasValuesAsync(Guid fieldId);
 
    // ---------------- Asset (data) ----------------
    Task<PaginatedResult<AssetDto>> GetAllAsync(PagingParams pagingParams, Guid? assetTypeId, Enums.AssetStatus? status);
    Task<PaginatedResult<AssetDto>> SearchAsync(AssetSearchRequest request);
    Task<AssetDto?> GetByIdAsync(Guid id);
    Task<List<AssetLookupDto>> GetLookupAsync(string? search, Guid? assetTypeId);
    Task<Asset?> GetEntityByIdAsync(Guid id);
 
    Task AddAsync(Asset asset);
    void Update(Asset asset);
    void SoftDelete(Asset asset);
 
    // ---------------- AssetAttributeValue (EAV values) ----------------
    Task ReplaceAttributeValuesAsync(Asset asset, Dictionary<string, object?> attributes, List<AssetTypeField> schema);
 
    // ---------------- CostAssetHist (maintenance) ----------------
    Task<List<CostAssetHist>> GetMaintenanceHistoryAsync(Guid assetId);
    Task AddMaintenanceRecordAsync(CostAssetHist record);
}

using System;
using static API.Entities.Enums;

namespace API.DTOs.Asset;

// ============================================================
//  Asset — list/detail view. "Attributes" is a flattened key->value map
//  built from AssetAttributeValue rows (or PropertiesJson), so the Angular
//  EAV field renderer can bind directly without knowing about the EAV tables.
// ============================================================
public class AssetDto
{
    public Guid Id { get; set; }
    public Guid AssetTypeId { get; set; }
    public string AssetTypeName { get; set; } = null!;
    public string Name { get; set; } = null!;
    public string? Notes { get; set; }
    public AcquisitionType AcquisitionType { get; set; }
    public decimal AcquisitionCost { get; set; }
    public decimal? MonthlyLeaseCost { get; set; }
    public AssetStatus Status { get; set; }
    public DateTime CreatedAt { get; set; }
 
    // key = AssetTypeField.Name (machine key), value = the actual stored value
    // (string/number/bool/date already coerced to its natural CLR type)
    public Dictionary<string, object?> Attributes { get; set; } = new();
}
 
public class AssetLookupDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public AssetStatus Status { get; set; }
}
 
public class AssetCreateDto
{
    public required Guid AssetTypeId { get; set; }
    public required string Name { get; set; }
    public string? Notes { get; set; }
    public AcquisitionType AcquisitionType { get; set; }
    public decimal AcquisitionCost { get; set; }
    public decimal? MonthlyLeaseCost { get; set; }
 
    // key = AssetTypeField.Name, value = raw value from the Angular form
    // (always sent as JSON; server parses/validates against the field's DataType)
    public Dictionary<string, object?> Attributes { get; set; } = new();
}
 
public class AssetUpdateDto
{
    public required string Name { get; set; }
    public string? Notes { get; set; }
    public AcquisitionType AcquisitionType { get; set; }
    public decimal AcquisitionCost { get; set; }
    public decimal? MonthlyLeaseCost { get; set; }
    public Dictionary<string, object?> Attributes { get; set; } = new();
    // AssetTypeId is intentionally NOT updatable — switching category would
    // invalidate the whole Attributes set against a different schema.
    // Create a new Asset under the new type instead.
}
 
public class AssetStatusUpdateDto
{
    public AssetStatus Status { get; set; }
}
 

public class CostAssetHistDto
{
    public Guid Id { get; set; }
    public DateTime Date { get; set; }
    public string Description { get; set; }=null!;
    public decimal Cost { get; set; }
    public string? MaintainedBy { get; set; }
}

public class CostAssetHistCreateDto
{
    public DateTime Date { get; set; }
    public string Description { get; set; }=null!;
    public decimal Cost { get; set; }
    public string? MaintainedBy { get; set; }
}

public class AssetAttributeUpdateDto
{
    // Must match an AssetTypeField.Name on this asset's AssetType.
    public required string FieldName { get; set; }
    public object? Value { get; set; }
}
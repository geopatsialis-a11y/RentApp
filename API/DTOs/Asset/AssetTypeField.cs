using System;
using static API.Entities.Enums;

namespace API.DTOs.Asset;


// ============================================================
//  AssetTypeField — the dynamic "schema" of a category
//  (e.g. for "Αυτοκίνητα": Mileage [Number], Color [Text/Select], FirstRegistration [Date])
// ============================================================
public class AssetTypeFieldDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;       // machine key, e.g. "mileage"
    public string Label { get; set; } = null!;       // UI label, e.g. "Χιλιόμετρα"
    public FieldDataType DataType { get; set; }
    public string? Placeholder { get; set; }
    public string? DefaultValue { get; set; }
    public int DisplayOrder { get; set; }
    public string? ValidationRegex { get; set; }
    public decimal? MinValue { get; set; }
    public decimal? MaxValue { get; set; }
    public bool IsRequired { get; set; }
    public List<AssetTypeFieldOptionDto> Options { get; set; } = new();
}
 
public class AssetTypeFieldCreateDto
{
    public required string Name { get; set; }
    public required string Label { get; set; }
    public FieldDataType DataType { get; set; }
    public string? Placeholder { get; set; }
    public string? DefaultValue { get; set; }
    public int DisplayOrder { get; set; }
    public string? ValidationRegex { get; set; }
    public decimal? MinValue { get; set; }
    public decimal? MaxValue { get; set; }
    public bool IsRequired { get; set; }
 
    // Only relevant when DataType is a "choice" type (e.g. Select) — see note in AssetService
    public List<AssetTypeFieldOptionCreateDto>? Options { get; set; }
}
 
public class AssetTypeFieldUpdateDto
{
    public required string Label { get; set; }
    public string? Placeholder { get; set; }
    public string? DefaultValue { get; set; }
    public int DisplayOrder { get; set; }
    public string? ValidationRegex { get; set; }
    public decimal? MinValue { get; set; }
    public decimal? MaxValue { get; set; }
    public bool IsRequired { get; set; }
    // Name and DataType are intentionally NOT editable after creation:
    // changing the machine key or type would orphan/corrupt existing
    // AssetAttributeValue rows and PropertiesJson across all assets of this type.
}
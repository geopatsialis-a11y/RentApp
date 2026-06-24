using System;

namespace API.DTOs.Asset;


// ============================================================
//  AssetTypeFieldOption — for Select/dropdown-style fields
//  (e.g. Color field -> options: "Κόκκινο"/red, "Μαύρο"/black)
// ============================================================
public class AssetTypeFieldOptionDto
{
    public Guid Id { get; set; }
    public string Label { get; set; } = null!;
    public string Value { get; set; } = null!;
    public int DisplayOrder { get; set; }
}
 
public class AssetTypeFieldOptionCreateDto
{
    public required string Label { get; set; }
    public required string Value { get; set; }
    public int DisplayOrder { get; set; }
}

public class AssetTypeFieldOptionUpdateDto
{
    public required string Label { get; set; }
    public required string Value { get; set; }
    public int DisplayOrder { get; set; }
}
using System;

namespace API.DTOs.Asset;

// ============================================================
//  AssetType — the "category" (e.g. "Αυτοκίνητα", "Βιβλία", "Σπίτια")
// ============================================================
public class AssetTypeDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
    public int AssetCount { get; set; }
    public List<AssetTypeFieldDto> Fields { get; set; } = new();
}
 
// Lightweight version for dropdowns (e.g. "select a category" when creating an Asset)
public class AssetTypeLookupDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
}
 
public class AssetTypeCreateDto
{
    public required string Name { get; set; }
    public string? Description { get; set; }
}
 
public class AssetTypeUpdateDto
{
    public required string Name { get; set; }
    public string? Description { get; set; }
}
 
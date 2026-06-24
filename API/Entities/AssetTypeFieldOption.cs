using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Entities;

public class AssetTypeFieldOption: BaseEntity
{
    [Required]
    public Guid AssetTypeFieldId { get; set; }
    
    [ForeignKey(nameof(AssetTypeFieldId))]
    public virtual AssetTypeField AssetTypeField { get; set; } = null!;

    [Required, MaxLength(200)]
    public required string Label { get; set; } // Π.χ., "Κόκκινο", "Μαύρο"

    [Required, MaxLength(200)]
    public required string Value { get; set; } // Π.χ., "red", "black" (για storage)

    [Range(0, int.MaxValue)]
    public int DisplayOrder { get; set; } = 0; // Για ταξινόμηση των επιλογών
}
using System;
using static API.Entities.Enums;

namespace API.DTOs.Asset;


// ============================================================
//  Dynamic search/filter request — the "eBay-style facet search"
//  Sent by Angular once the user has picked an AssetType and is
//  filling in the dynamically-rendered filter panel for that category.
// ============================================================
public class AssetSearchRequest
{
    public required Guid AssetTypeId { get; set; }
    public AssetStatus? Status { get; set; }
    
    public string? Search { get; set; }

    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 20;

    public string? SortBy { get; set; }
 
    // One entry per filtered field. All entries are combined with AND.
    public List<AssetAttributeFilter> Filters { get; set; } = new();
}
 
public class AssetAttributeFilter
{
    // Must match an AssetTypeField.Name belonging to AssetSearchRequest.AssetTypeId —
    // validated server-side against that type's schema before building the query.
    public required string FieldName { get; set; }
 
    // For Text/Select/Boolean fields: exact-match equality.
    public string? Equals { get; set; }
 
    // For Number/Date fields: inclusive range. Either bound may be omitted
    // for an open-ended range (e.g. "mileage <= 50000" -> Max only).
    public decimal? MinValue { get; set; }
    public decimal? MaxValue { get; set; }
    public DateTime? MinDate { get; set; }
    public DateTime? MaxDate { get; set; }
}
 
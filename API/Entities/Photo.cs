using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace API.Entities;

public class Photo
{
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();

    [Required]
    public required Guid TenantId { get; set; }

    [Required, MaxLength(1000)]
    public required string Url { get; set; }
    [MaxLength(255)]
    public string? PublicId { get; set; }
    public bool IsMain { get; set; }


    //navigation property
    [JsonIgnore]
    public Asset Asset { get; set; }=null!;
    public Guid AssetId{ get; set; }
}

   


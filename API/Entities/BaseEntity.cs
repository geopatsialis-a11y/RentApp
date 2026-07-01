using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using API.Interfaces;

namespace API.Entities;

public abstract class BaseEntity : IMustHaveTenant
{
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();

    [Required]
    public required Guid TenantId { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    
    [MaxLength(50)]
    public string CreatedBy { get; set; } = string.Empty;

    public DateTime? UpdatedAt { get; set; }
    
    [MaxLength(50)]
    public string? UpdatedBy { get; set; }

    public bool IsDeleted { get; set; } = false;
    public DateTime? DeletedAt { get; set; }
    
    [MaxLength(50)]
    public string? DeletedBy { get; set; }

    // Θα ρυθμιστεί μέσω Fluent API για το xmin της PostgreSQL
     public uint xmin { get; private set; }

     // Navigation Properties
     [ForeignKey(nameof(TenantId))]
    public  Tenant Tenant { get; set; } = null!;

    [ForeignKey(nameof(CreatedBy))]
    public Member CreatedByMember { get; set; } = null!;

    [ForeignKey(nameof(UpdatedBy))]
    public Member? UpdatedByMember { get; set; }

    [ForeignKey(nameof(DeletedBy))]
    public Member? DeletedByMember { get; set; }

}

using System;

namespace API.DTOs.Contacts;

public class ContactDto
{

    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public string? Phone { get; set; }
    public string? Email { get; set; }
    public bool CanUseAsset { get; set; }
    public string? Notes { get; set; }
}

public class ContactCreateDto
{
    
    public required string Name { get; set; }
    public string? Phone { get; set; }
    public string? Email { get; set; }
    public bool CanUseAsset { get; set; }
    public string? Notes { get; set; }

}
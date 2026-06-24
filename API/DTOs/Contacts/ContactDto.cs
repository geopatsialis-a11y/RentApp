using System;

namespace API.DTOs.Contacts;

public class ContactDto
{

    public Guid Id { get; set; }
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string? Phone { get; set; }
    public string? Email { get; set; }
    public bool CanUseAsset { get; set; }
    public string? Notes { get; set; }
}

public class ContactCreateDto
{
    
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public string? Phone { get; set; }
    public string? Email { get; set; }
    public bool CanUseAsset { get; set; }
    public string? Notes { get; set; }

}
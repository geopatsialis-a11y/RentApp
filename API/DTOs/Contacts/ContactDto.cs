using System;

namespace API.DTOs.Contacts;

public class ContactDto
{
    public uint RowVersion { get; set; }
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public string? Phone { get; set; }
    public string? Email { get; set; }
    public bool CanUseAsset { get; set; }
    public string? Notes { get; set; }
}

public class ContactCreateDto
{
    public uint RowVersion { get; set; }
    public required string Name { get; set; }
    public string? Phone { get; set; }
    public string? Email { get; set; }
    public bool CanUseAsset { get; set; }
    public string? Notes { get; set; }

}


public class ContactUpdateDto
{
    public uint RowVersion { get; set; }
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public string? Phone { get; set; }
    public string? Email { get; set; }
    public bool CanUseAsset { get; set; }
    public string? Notes { get; set; }
}
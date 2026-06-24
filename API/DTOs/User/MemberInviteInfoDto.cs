using System;

namespace API.DTOs;

public class MemberInviteInfoDto
{
    public required string Email { get; set; }
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public required string TenantName { get; set; }
}

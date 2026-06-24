using System;
using System.Security.Claims;

namespace API.Extensions;

public static class ClaimsPrincipalExtensions
{
    public static Guid GetMemberId(this ClaimsPrincipal user)
    {
        var userId= user.FindFirstValue(ClaimTypes.NameIdentifier) ??
            throw new Exception("Cannot get User id from claims - token");
        
        if (!Guid.TryParse(userId, out var guid))
            throw new Exception("Invalid User id in token");

        return guid;
    }

    public static Guid GetTenantId(this ClaimsPrincipal user)
    {
        var tenantId = user.FindFirstValue("TenantId")
        ?? throw new Exception("Cannot get tenant id from claims - token");

        if (!Guid.TryParse(tenantId, out var guid))
            throw new Exception("Invalid tenant id in token");

        return guid;
    }

}

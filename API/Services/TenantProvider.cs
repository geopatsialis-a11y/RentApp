using System;
using API.Interfaces;

namespace API.Services;

public class TenantProvider(IHttpContextAccessor httpContextAccessor): ITenantProvider
{
    private Guid? _manualTenantId; 

    public void SetCurrentTenant(Guid tenantId)
    {
        _manualTenantId = tenantId;
    }

    public Guid TenantId
    {
        get
        {
            // Αν έχει τεθεί χειροκίνητα (π.χ. κατά το login), επέστρεψε αυτό
            if (_manualTenantId.HasValue)
                return _manualTenantId.Value;
            var value = httpContextAccessor.HttpContext?.User?
                .FindFirst("TenantId")?.Value;

            if (!Guid.TryParse(value, out var tenantId))
            {
                throw new Exception("TenantProvider - TenantId not found.");
            }

            return tenantId;
        } }
}
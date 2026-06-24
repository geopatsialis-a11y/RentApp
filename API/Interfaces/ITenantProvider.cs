using System;

namespace API.Interfaces;

public interface ITenantProvider
{
    Guid TenantId { get; }
    void SetCurrentTenant(Guid tenantId);   
}

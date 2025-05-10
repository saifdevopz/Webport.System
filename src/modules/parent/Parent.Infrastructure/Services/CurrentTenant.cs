using Common.Domain.Extensions;
using Common.Infrastructure.Authentication;
using Microsoft.AspNetCore.Http;

namespace Parent.Infrastructure.Services;

public sealed class CurrentTenant(IHttpContextAccessor httpContextAccessor)
{
    public string? TenantId => httpContextAccessor.HttpContext?.Request.Headers["Tenant"];
    public string? TenantDbName => httpContextAccessor.HttpContext?.User.GetTenantDbName();

    public string GetTenantConnectionString()
    {
        if (string.IsNullOrWhiteSpace(TenantDbName))
        {
            return GeneralExtensions.BuildConnectionString("parent-db");
        }

        return GeneralExtensions.BuildConnectionString(TenantDbName);
    }

}

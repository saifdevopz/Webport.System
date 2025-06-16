using Common.Domain.Extensions;
using Common.Infrastructure.Authentication;
using Common.Infrastructure.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace Common.Infrastructure.Database;

public sealed class CurrentConnection(IHttpContextAccessor httpContextAccessor, IConfiguration config)
{
    private readonly string _parentConnectionString = config.GetValueOrThrow<string>("PostgreSQL:DefaultConnection");
    public string? TenantId => httpContextAccessor.HttpContext?.Request.Headers["Tenant"];
    public string? TenantDbName => httpContextAccessor.HttpContext?.User.GetTenantDbName();
    public string? UserEmail => httpContextAccessor.HttpContext?.User.GetUserEmail();

    public string GetParentConnectionString()
    {
        if (string.IsNullOrWhiteSpace(TenantDbName))
        {
            return _parentConnectionString;
        }

        return GeneralExtensions.BuildConnectionString(TenantDbName);
    }
}
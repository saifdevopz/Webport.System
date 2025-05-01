using System.Domain.Models;
using System.Linq.Expressions;

namespace System.Domain.DTOs.Tenants;

public static class TenantQueries
{
    public static Expression<Func<TenantM, TenantDto>> ProjectToDto()
    {
        return _ => new TenantDto
        {
            TenantName = _.TenantName,
            DatabaseName = _.DatabaseName
        };
    }
}

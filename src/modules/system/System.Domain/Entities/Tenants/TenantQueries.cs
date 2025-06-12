using System.Linq.Expressions;

namespace System.Domain.Entities.Tenants;

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

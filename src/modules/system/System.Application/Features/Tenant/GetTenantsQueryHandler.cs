using System.Domain.Models;

namespace System.Application.Features.Tenant;

internal sealed class GetTenantsQueryHandler(IGenericRepository<TenantM> repository)
    : IQueryHandler<GetTenantsQuery, GetTenantsQueryResult>
{
    public async Task<Result<GetTenantsQueryResult>> Handle(
        GetTenantsQuery query,
        CancellationToken cancellationToken)
    {
        var obj = await repository.GetAllAsync(cancellationToken);

        return Result.Success(new GetTenantsQueryResult(obj));
    }
}

public sealed record GetTenantsQuery : IQuery<GetTenantsQueryResult>;

public sealed record GetTenantsQueryResult(IEnumerable<TenantM> Tenants);




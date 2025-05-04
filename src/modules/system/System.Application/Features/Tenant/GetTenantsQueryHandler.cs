using Common.Application.CQRS;
using Common.Domain.Results;
using System.Application.Interfaces;
using System.Domain.Models;

namespace System.Application.Features.Tenant;

public class GetTenantsQueryHandler(IRepository<TenantM> repository)
    : IQueryHandler<GetTenantsQuery, Result<GetTenantsQueryResult>>
{
    public async Task<Result<GetTenantsQueryResult>> Handle(
        GetTenantsQuery query,
        CancellationToken cancellation = default)
    {
        var obj = await repository.GetAllAsync(cancellation);

        return Result.Success(new GetTenantsQueryResult(obj));
    }
}

public sealed record GetTenantsQuery;

public sealed record GetTenantsQueryResult(IEnumerable<TenantM> Tenants);
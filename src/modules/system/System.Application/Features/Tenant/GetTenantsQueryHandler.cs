using Common.Application.CQRS;
using Common.Application.Interfaces;
using Common.Domain.Results;
using System.Domain.Models;

namespace System.Application.Features.Tenant;

public class GetTenantsQueryHandler(IGenericRepository<TenantM> repository)
    : IQueryHandler<GetTenantsQuery, Result<List<TenantM>>>
{
    public async Task<Result<List<TenantM>>> Handle(
        GetTenantsQuery query,
        CancellationToken cancellation = default)
    {
        var obj = await repository.GetAllAsync(cancellation);

        return Result.Success(obj);
    }
}

public sealed record GetTenantsQuery;

public sealed record GetTenantsQueryResult(IEnumerable<TenantM> Tenants);
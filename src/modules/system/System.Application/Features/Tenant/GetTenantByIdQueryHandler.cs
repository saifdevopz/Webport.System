using System.Domain.Models;

namespace System.Application.Features.Tenant;

public class GetTenantByIdQueryHandler(IGenericRepository<TenantM> repository)
    : IQueryHandler<GetTenantByIdQuery, GetTenantByIdQueryResult>
{
    public async Task<Result<GetTenantByIdQueryResult>> Handle(
        GetTenantByIdQuery query,
        CancellationToken cancellationToken)
    {
        var obj = await repository.FindOneAsync(_ => _.TenantId == query.TenantId, cancellationToken);

        return obj is not null
            ? Result.Success(new GetTenantByIdQueryResult(obj))
            : Result.Failure<GetTenantByIdQueryResult>(CustomError.NotFound("Not Found", "Tenant not found."));
    }
}

public sealed record GetTenantByIdQuery(int TenantId) : IQuery<GetTenantByIdQueryResult>;

public sealed record GetTenantByIdQueryResult(TenantM Tenant);
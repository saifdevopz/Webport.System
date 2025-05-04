using Common.Application.CQRS;
using Common.Domain.Errors;
using Common.Domain.Results;
using System.Application.Interfaces;
using System.Domain.Models;

namespace System.Application.Features.Tenant;

public class GetTenantByIdQueryHandler(IRepository<TenantM> repository)
    : IQueryHandler<GetTenantByIdQuery, Result<GetTenantByIdQueryResult>>
{
    public async Task<Result<GetTenantByIdQueryResult>> Handle(
        GetTenantByIdQuery query,
        CancellationToken cancellation = default)
    {
        var obj = await repository.FindOneAsync(_ => _.TenantId == query.TenantId, cancellation);

        return obj is not null
            ? Result.Success(new GetTenantByIdQueryResult(obj))
            : Result.Failure<GetTenantByIdQueryResult>(CustomError.NotFound("Not Found", "Tenant not found."));
    }
}

public sealed record GetTenantByIdQuery(int TenantId);

public sealed record GetTenantByIdQueryResult(TenantM Tenant);
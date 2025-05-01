using Common.Application.CQRS;
using Common.Domain.Results;
using System.Application.Interfaces;
using System.Domain.Models;

namespace System.Application.Features.Roles;

public class GetRolesQueryHandler(IRepository<RoleM> repository)
    : IQueryHandler<GetRolesQuery, Result<GetRolesQueryResult>>
{
    public async Task<Result<GetRolesQueryResult>> Handle(
        GetRolesQuery query,
        CancellationToken cancellation = default)
    {
        var Obj = await repository.GetAllAsync(cancellation);

        return Result.Success(new GetRolesQueryResult(Obj));
    }
}

public sealed record GetRolesQuery;

public sealed record GetRolesQueryResult(IEnumerable<RoleM> Roles);
using Common.Application.CQRS;
using Common.Application.Interfaces;
using Common.Domain.Errors;
using Common.Domain.Results;
using System.Domain.Models;

namespace System.Application.Features.Roles;

public class GetRoleByIdQueryHandler(IGenericRepository<RoleM> repository)
    : IQueryHandler<GetRoleByIdQuery, Result<GetRoleByIdQueryResult>>
{
    public async Task<Result<GetRoleByIdQueryResult>> Handle(
        GetRoleByIdQuery query,
        CancellationToken cancellation = default)
    {
        var obj = await repository.FindOneAsync(_ => _.RoleId == query.RoleId, cancellation);

        return obj is not null
            ? Result.Success(new GetRoleByIdQueryResult(obj))
            : Result.Failure<GetRoleByIdQueryResult>(CustomError.NotFound("Not Found", "Role not found."));
    }
}

public sealed record GetRoleByIdQuery(int RoleId);

public sealed record GetRoleByIdQueryResult(RoleM Role);

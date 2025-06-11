using System.Domain.Entities.Roles;

namespace System.Application.Features.Roles;

public class GetRoleByIdQueryHandler(IGenericRepository<RoleM> repository)
    : IQueryHandler<GetRoleByIdQuery, GetRoleByIdQueryResult>
{
    public async Task<Result<GetRoleByIdQueryResult>> Handle(
        GetRoleByIdQuery query,
        CancellationToken cancellationToken)
    {
        var obj = await repository.FindOneAsync(_ => _.RoleId == query.RoleId, cancellationToken);

        return obj is not null
            ? Result.Success(new GetRoleByIdQueryResult(obj))
            : Result.Failure<GetRoleByIdQueryResult>(CustomError.NotFound("Not Found", "Role not found."));
    }
}

public sealed record GetRoleByIdQuery(int RoleId) : IQuery<GetRoleByIdQuery>;

public sealed record GetRoleByIdQueryResult(RoleM Role);

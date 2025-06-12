using System.Domain.Entities.Roles;

namespace System.Application.Features.Roles;

public class GetRolesQueryHandler(IGenericRepository<RoleM> repository)
    : IQueryHandler<GetRolesQuery, GetRolesQueryResult>
{
    public async Task<Result<GetRolesQueryResult>> Handle(
        GetRolesQuery query,
        CancellationToken cancellationToken)
    {
        var obj = await repository.GetAllAsync(cancellationToken);

        return Result.Success(new GetRolesQueryResult(obj));
    }
}

public sealed record GetRolesQuery : IQuery<GetRolesQueryResult>;

public sealed record GetRolesQueryResult(IEnumerable<RoleM> Roles);
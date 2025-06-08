using System.Domain.Models;

namespace System.Application.Features.Users;

public class GetUsersQueryHandler(IGenericRepository<UserM> repository)
    : IQueryHandler<GetUsersQuery, GetUsersQueryResult>
{
    public async Task<Result<GetUsersQueryResult>> Handle(
        GetUsersQuery query,
        CancellationToken cancellationToken)
    {
        var obj = await repository.GetAllAsync(cancellationToken);

        return Result.Success(new GetUsersQueryResult(obj));
    }
}

public sealed record GetUsersQuery : IQuery<GetUsersQueryResult>;

public sealed record GetUsersQueryResult(IEnumerable<UserM> Users);
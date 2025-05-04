using Common.Application.CQRS;
using Common.Domain.Results;
using System.Application.Interfaces;
using System.Domain.Models;

namespace System.Application.Features.Users;

public class GetUsersQueryHandler(IRepository<UserM> repository)
    : IQueryHandler<GetUsersQuery, Result<GetUsersQueryResult>>
{
    public async Task<Result<GetUsersQueryResult>> Handle(
        GetUsersQuery query,
        CancellationToken cancellation = default)
    {
        var obj = await repository.GetAllAsync(cancellation);

        return Result.Success(new GetUsersQueryResult(obj));
    }
}

public sealed record GetUsersQuery;

public sealed record GetUsersQueryResult(IEnumerable<UserM> Users);
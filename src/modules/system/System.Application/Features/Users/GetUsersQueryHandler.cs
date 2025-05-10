using Common.Application.CQRS;
using Common.Application.Interfaces;
using Common.Domain.Results;
using System.Domain.Models;

namespace System.Application.Features.Users;

public class GetUsersQueryHandler(IGenericRepository<UserM> repository)
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
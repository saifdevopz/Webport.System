using Common.Application.CQRS;
using Common.Application.Interfaces;
using Common.Domain.Errors;
using Common.Domain.Results;
using System.Domain.Models;

namespace System.Application.Features.Users;

public class GetUserByIdQueryHandler(IGenericRepository<UserM> repository)
    : IQueryHandler<GetUserByIdQuery, Result<GetUserByIdQueryResult>>
{
    public async Task<Result<GetUserByIdQueryResult>> Handle(
        GetUserByIdQuery query,
        CancellationToken cancellation = default)
    {
        var obj = await repository.FindOneAsync(_ => _.UserId == query.UserId, cancellation);

        return obj is not null
            ? Result.Success(new GetUserByIdQueryResult(obj))
            : Result.Failure<GetUserByIdQueryResult>(CustomError.NotFound("Not Found", "User not found."));
    }
}

public sealed record GetUserByIdQuery(int UserId);

public sealed record GetUserByIdQueryResult(UserM User);
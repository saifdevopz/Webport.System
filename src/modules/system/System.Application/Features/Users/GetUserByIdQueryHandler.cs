using System.Domain.Entities.Users;

namespace System.Application.Features.Users;

public class GetUserByIdQueryHandler(IGenericRepository<UserM> repository)
    : IQueryHandler<GetUserByIdQuery, GetUserByIdQueryResult>
{
    public async Task<Result<GetUserByIdQueryResult>> Handle(
        GetUserByIdQuery query,
        CancellationToken cancellationToken)
    {
        var obj = await repository.FindOneAsync(_ => _.UserId == query.UserId, cancellationToken);

        return obj is not null
            ? Result.Success(new GetUserByIdQueryResult(obj))
            : Result.Failure<GetUserByIdQueryResult>(CustomError.NotFound("Not Found", "User not found."));
    }
}

public sealed record GetUserByIdQuery(int UserId) : IQuery<GetUserByIdQueryResult>;

public sealed record GetUserByIdQueryResult(UserM User);
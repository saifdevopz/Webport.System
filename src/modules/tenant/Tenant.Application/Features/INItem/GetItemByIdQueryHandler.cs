using Tenant.Domain.Entities.INItem;

namespace Tenant.Application.Features.INItem;

public class GetItemByIdQueryHandler(IGenericRepository<INItemM> repository)
    : IQueryHandler<GetItemByIdQuery, GetItemByIdQueryResult>
{
    public async Task<Result<GetItemByIdQueryResult>> Handle(
        GetItemByIdQuery query,
        CancellationToken cancellationToken)
    {
        var obj = await repository.FindOneAsync(_ => _.ItemId == query.ItemId, cancellationToken);

        return obj is not null
            ? Result.Success(new GetItemByIdQueryResult(obj))
            : Result.Failure<GetItemByIdQueryResult>(CustomError.NotFound("Not Found", "Record not found."));
    }
}

public sealed record GetItemByIdQuery(int ItemId) : IQuery<GetItemByIdQuery>;

public sealed record GetItemByIdQueryResult(INItemM Obj);

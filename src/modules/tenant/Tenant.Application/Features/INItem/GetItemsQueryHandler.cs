using Tenant.Domain.Entities.INItem;

namespace Tenant.Application.Features.INItem;

public class GetItemsQueryHandler(IGenericRepository<INItemM> repository)
    : IQueryHandler<GetItemsQuery, GetItemsQueryResult>
{
    public async Task<Result<GetItemsQueryResult>> Handle(
        GetItemsQuery query,
        CancellationToken cancellationToken)
    {
        var obj = await repository.GetAllAsync(cancellationToken);

        return Result.Success(new GetItemsQueryResult(obj));
    }
}

public sealed record GetItemsQuery : IQuery<GetItemsQueryResult>;

public sealed record GetItemsQueryResult(IEnumerable<INItemM> Obj);
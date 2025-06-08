
using Parent.Domain.Inventory.Category;

namespace Parent.Application.Features.Category;

public class GetAllCategoryQueryHandler(IGenericRepository<CategoryM> repository)
    : IQueryHandler<GetAllCategoryQuery, GetAllCategoryQueryResult>
{
    public async Task<Result<GetAllCategoryQueryResult>> Handle(
        GetAllCategoryQuery query,
        CancellationToken cancellationToken)
    {
        var obj = await repository.GetAllAsync(cancellationToken);

        return Result.Success(new GetAllCategoryQueryResult(obj));
    }
}

public sealed record GetAllCategoryQuery : IQuery<GetAllCategoryQueryResult>;

public sealed record GetAllCategoryQueryResult(IEnumerable<CategoryM> Categories);
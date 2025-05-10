using Common.Application.CQRS;
using Common.Application.Interfaces;
using Common.Domain.Results;
using Parent.Domain.Inventory.Category;

namespace Parent.Application.Features.Category;

public class GetAllCategoryQueryHandler(IGenericRepository<CategoryM> repository)
    : IQueryHandler<GetAllCategoryQuery, Result<GetAllCategoryQueryResult>>
{
    public async Task<Result<GetAllCategoryQueryResult>> Handle(
        GetAllCategoryQuery query,
        CancellationToken cancellation = default)
    {
        var obj = await repository.GetAllAsync(cancellation);

        return Result.Success(new GetAllCategoryQueryResult(obj));
    }
}

public sealed record GetAllCategoryQuery;

public sealed record GetAllCategoryQueryResult(IEnumerable<CategoryM> Categories);
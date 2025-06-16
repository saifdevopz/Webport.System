using Tenant.Domain.Entities.INCategory;

namespace Tenant.Application.Features.INCategory;

public class GetCategoriesQueryHandler(IGenericRepository<INCategoryM> repository)
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

public sealed record GetAllCategoryQueryResult(IEnumerable<INCategoryM> Categories);
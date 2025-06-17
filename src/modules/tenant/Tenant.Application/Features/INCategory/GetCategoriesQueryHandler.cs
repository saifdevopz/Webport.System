using Tenant.Domain.Entities.INCategory;

namespace Tenant.Application.Features.INCategory;

public class GetCategoriesQueryHandler(IGenericRepository<INCategoryM> Repository)
    : IQueryHandler<GetCategoriesQuery, GetCategoriesQueryResult>
{
    public async Task<Result<GetCategoriesQueryResult>> Handle(
        GetCategoriesQuery query,
        CancellationToken cancellationToken)
    {
        var obj = await Repository.GetAllAsync(cancellationToken);

        return Result.Success(new GetCategoriesQueryResult(obj));
    }
}

public sealed record GetCategoriesQuery : IQuery<GetCategoriesQueryResult>;

public sealed record GetCategoriesQueryResult(IEnumerable<INCategoryM> Categories);
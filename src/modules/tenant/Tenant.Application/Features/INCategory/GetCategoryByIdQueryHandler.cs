using Tenant.Domain.Entities.INCategory;

namespace Tenant.Application.Features.INCategory;

public class GetCategoryByIdQueryHandler(IGenericRepository<INCategoryM> Repository)
    : IQueryHandler<GetCategoryByIdQuery, GetCategoryByIdQueryResult>
{
    public async Task<Result<GetCategoryByIdQueryResult>> Handle(
        GetCategoryByIdQuery query,
        CancellationToken cancellationToken)
    {
        var obj = await Repository.FindOneAsync(_ => _.CategoryId == query.CategoryId, cancellationToken);

        return obj is not null
            ? Result.Success(new GetCategoryByIdQueryResult(obj))
            : Result.Failure<GetCategoryByIdQueryResult>(CustomError.NotFound("Not Found", "Record not found."));
    }
}

public sealed record GetCategoryByIdQuery(int CategoryId) : IQuery<GetCategoryByIdQuery>;

public sealed record GetCategoryByIdQueryResult(INCategoryM Category);

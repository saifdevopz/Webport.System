using Parent.Application.Features.Category;
using Parent.Presentation.Common;

namespace Parent.Presentation.Endpoints.Category;

internal sealed class GetAllCategoryEndpoint(IQueryDispatcher _sender) : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("category", async () =>
        {
            var response = await _sender
                .Dispatch<GetAllCategoryQuery, Result<GetAllCategoryQueryResult>>(new GetAllCategoryQuery())
                .MapResult();

            return response;
        })
        .WithTags(Tags.Category)
        .RequireAuthorization();
    }
}
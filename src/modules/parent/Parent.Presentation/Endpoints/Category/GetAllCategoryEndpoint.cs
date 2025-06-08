using Common.Application.Messaging;
using Parent.Application.Features.Category;
using Parent.Presentation.Common;

namespace Parent.Presentation.Endpoints.Category;

internal sealed class GetAllCategoryEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("category", async (
            IQueryHandler<GetAllCategoryQuery, GetAllCategoryQueryResult> handler,
            CancellationToken cancellationToken) =>
        {
            var response = await handler.Handle(new GetAllCategoryQuery(), cancellationToken)
                                        .MapResult();

            return response;
        })
        .WithTags(Tags.Category)
        .RequireAuthorization();
    }
}
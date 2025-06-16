using Tenant.Application.Features.INItem;

namespace Tenant.Presentation.Endpoints.INItem;

internal sealed class GetItemsEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("item", async (
            IQueryHandler<GetItemsQuery, GetItemsQueryResult> handler,
            CancellationToken cancellationToken) =>
        {
            var response = await handler
                .Handle(new GetItemsQuery(), cancellationToken)
                .MapResult();

            return response;
        })
        .WithTags(Tags.Item)
        .RequireAuthorization();
    }
}
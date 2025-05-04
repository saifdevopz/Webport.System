using System.Application.Features.Tenant;

namespace System.Presentation.Endpoints.Tenant;

internal sealed class GetTenantsEndpoint(IQueryDispatcher _sender) : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("tenant", async () =>
        {
            var response = await _sender
                .Dispatch<GetTenantsQuery, Result<GetTenantsQueryResult>>(new GetTenantsQuery())
                .MapResult();

            return response;
        })
        .WithTags(Tags.Tenant);
    }
}
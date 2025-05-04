using System.Application.Features.Tenant;

namespace System.Presentation.Endpoints.Tenant;

internal sealed class GetTenantByIdEndpoint(IQueryDispatcher _sender) : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("tenant/{Id}", async (int Id) =>
        {
            var response = await _sender
                .Dispatch<GetTenantByIdQuery, Result<GetTenantByIdQueryResult>>(new GetTenantByIdQuery(Id))
                .MapResult();

            return response;
        })
        .WithTags(Tags.Tenant);
    }
}
using System.Application.Features.Tenant;

namespace System.Presentation.Endpoints.Tenant;

internal sealed class CreateTenantEndpoint(ICommandDispatcher _sender) : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("tenant", async (CreateTenantCommand request) =>
        {
            var response = await _sender
                .Dispatch<CreateTenantCommand, Result>(request)
                .MapResult();

            return response;
        })
        .WithTags(Tags.Tenant);
    }
}
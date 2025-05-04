using System.Application.Features.Tenant;

namespace System.Presentation.Endpoints.Tenant;

internal sealed class UpdateTenantEndpoint(ICommandDispatcher _sender) : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPut("tenant", async (UpdateTenantCommand request) =>
        {
            var response = await _sender
                .Dispatch<UpdateTenantCommand, Result>(request)
                .MapResult();

            return response;
        })
        .WithTags(Tags.Tenant);
    }
}
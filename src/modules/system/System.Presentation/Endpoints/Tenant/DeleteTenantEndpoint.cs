using System.Application.Features.Tenant;

namespace System.Presentation.Endpoints.Tenant;

internal sealed class DeleteTenantEndpoint(ICommandDispatcher _sender) : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapDelete("tenant/{Id}", async (int Id) =>
        {
            var response = await _sender
                .Dispatch<DeleteTenantCommand, Result>(new DeleteTenantCommand(Id))
                .MapResult();

            return response;
        })
        .WithTags(Tags.Tenant);
    }
}
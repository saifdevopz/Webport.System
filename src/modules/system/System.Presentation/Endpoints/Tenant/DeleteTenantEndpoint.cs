using System.Application.Features.Tenant;

namespace System.Presentation.Endpoints.Tenant;

internal sealed class DeleteTenantEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapDelete("tenant/{Id}", async (
            int Id,
            ICommandHandler<DeleteTenantCommand> handler,
            CancellationToken cancellationToken) =>
        {
            var response = await handler
                .Handle(new DeleteTenantCommand(Id), cancellationToken)
                .MapResult();

            return response;
        })
        .WithTags(Tags.Tenant);
    }
}
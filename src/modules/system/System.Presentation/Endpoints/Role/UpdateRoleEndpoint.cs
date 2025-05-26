using System.Application.Features.Roles;

namespace System.Presentation.Endpoints.Role;

internal sealed class UpdateRoleEndpoint(ICommandDispatcher _sender) : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPut("role", async (UpdateRoleCommand request) =>
        {
            var response = await _sender
                .Dispatch<UpdateRoleCommand, Result>(request)
                .MapResult();

            return response;
        })
        .WithTags(Tags.Role)
        .RequireAuthorization();
    }
}
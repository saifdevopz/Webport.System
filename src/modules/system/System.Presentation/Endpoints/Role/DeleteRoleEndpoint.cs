using System.Application.Features.Roles;

namespace System.Presentation.Endpoints.Role;

internal sealed class DeleteRoleEndpoint(ICommandDispatcher _sender) : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapDelete("role/{Id}", async (int Id) =>
        {
            var response = await _sender
                .Dispatch<DeleteRoleCommand, Result>(new DeleteRoleCommand(Id))
                .MapResult();

            return response;
        })
        .WithTags(Tags.Role)
        .RequireAuthorization();
    }
}
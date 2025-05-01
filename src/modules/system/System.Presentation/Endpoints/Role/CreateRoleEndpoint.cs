using System.Application.Features.Roles;

namespace System.Presentation.Endpoints.Role;

internal sealed class CreateRoleEndpoint(ICommandDispatcher _sender) : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("role", async (CreateRoleCommand request) =>
        {
            var response = await _sender
                .Dispatch<CreateRoleCommand, Result>(request)
                .MapResult();

            return response;
        })
        .WithTags(Tags.Role);
    }
}
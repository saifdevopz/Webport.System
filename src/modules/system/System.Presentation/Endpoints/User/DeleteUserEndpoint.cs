using System.Application.Features.Users;

namespace System.Presentation.Endpoints.User;

internal sealed class DeleteUserEndpoint(ICommandDispatcher _sender) : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapDelete("user/{Id}", async (int Id) =>
        {
            var response = await _sender
                .Dispatch<DeleteUserCommand, Result>(new DeleteUserCommand(Id))
                .MapResult();

            return response;
        })
        .WithTags(Tags.User);
    }
}
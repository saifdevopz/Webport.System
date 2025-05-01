using System.Application.Features.Users;

namespace System.Presentation.Endpoints.User;

internal sealed class CreateUserEndpoint(ICommandDispatcher _sender) : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("user", async (CreateUserCommand request) =>
        {
            var response = await _sender
                .Dispatch<CreateUserCommand, Result>(request)
                .MapResult();

            return response;
        })
        .WithTags(Tags.User);
    }
}

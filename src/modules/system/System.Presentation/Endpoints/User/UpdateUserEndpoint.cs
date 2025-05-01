using System.Application.Features.Users;

namespace System.Presentation.Endpoints.User;

internal sealed class UpdateUserEndpoint(ICommandDispatcher _sender) : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPut("user", async (UpdateUserCommand request) =>
        {
            var response = await _sender
                .Dispatch<UpdateUserCommand, Result>(request)
                .MapResult();

            return response;
        })
        .WithTags(Tags.User);
    }
}
using System.Application.Features.Token;

namespace System.Presentation.Endpoints.Token;

internal sealed class AccessTokenEndpoint(ICommandDispatcher _sender) : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("access", async (AccessTokenCommand request) =>
        {
            var response = await _sender
                .Dispatch<AccessTokenCommand, Result<AccessTokenResult>>(request)
                .MapResult();

            return response;
        })
        .WithTags(Tags.Token);
    }
}

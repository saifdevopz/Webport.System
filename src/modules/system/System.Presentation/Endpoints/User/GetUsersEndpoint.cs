using System.Application.Features.Users;

namespace System.Presentation.Endpoints.User;

internal sealed class GetUsersEndpoint(IQueryDispatcher _sender) : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("user", async () =>
        {
            var response = await _sender
                .Dispatch<GetUsersQuery, Result<GetUsersQueryResult>>(new GetUsersQuery())
                .MapResult();

            return response;
        })
        .WithTags(Tags.User);
    }
}

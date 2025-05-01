using System.Application.Features.Users;

namespace System.Presentation.Endpoints.User;

internal sealed class GetUsersByIdEndpoint(IQueryDispatcher _sender) : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("user/{Id}", async (int Id) =>
        {
            var response = await _sender
                .Dispatch<GetUserByIdQuery, Result<GetUserByIdQueryResult>>(new GetUserByIdQuery(Id))
                .MapResult();

            return response;
        })
        .WithTags(Tags.User);
    }
}

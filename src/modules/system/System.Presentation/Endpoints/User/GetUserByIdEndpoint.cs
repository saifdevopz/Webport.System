using System.Application.Features.Users;

namespace System.Presentation.Endpoints.User;

internal sealed class GetUsersByIdEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("user/{Id}", async (
            int Id,
            IQueryHandler<GetUserByIdQuery, GetUserByIdQueryResult> handler,
            CancellationToken cancellationToken) =>
        {
            var response = await handler
                .Handle(new GetUserByIdQuery(Id), cancellationToken)
                .MapResult();

            return response;
        })
        .WithTags(Tags.User);
    }
}

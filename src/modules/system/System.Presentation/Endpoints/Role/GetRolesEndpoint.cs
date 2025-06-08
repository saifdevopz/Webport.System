using System.Application.Features.Roles;

namespace System.Presentation.Endpoints.Role;

internal sealed class GetRolesEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("role", async (
            IQueryHandler<GetRolesQuery, GetRolesQueryResult> handler,
            CancellationToken cancellation) =>
        {
            var response = await handler
                .Handle(new GetRolesQuery(), cancellation)
                .MapResult();

            return response;
        })
        .WithTags(Tags.Role);
        //.RequireAuthorization();
    }
}
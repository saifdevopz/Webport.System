using System.Application.Features.Roles;

namespace System.Presentation.Endpoints.Role;

internal sealed class GetRolesEndpoint(IQueryDispatcher _sender) : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("role", async () =>
        {
            var response = await _sender
                .Dispatch<GetRolesQuery, Result<GetRolesQueryResult>>(new GetRolesQuery());

            return response;
        })
        .WithTags(Tags.Role)
        .RequireAuthorization();
    }
}
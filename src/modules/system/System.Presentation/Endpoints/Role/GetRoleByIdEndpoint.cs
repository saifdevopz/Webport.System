using System.Application.Features.Roles;

namespace System.Presentation.Endpoints.Role;

internal sealed class GetRoleByIdEndpoint(IQueryDispatcher _sender) : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("role/{Id}", async (int Id) =>
        {
            var response = await _sender
                .Dispatch<GetRoleByIdQuery, Result<GetRoleByIdQueryResult>>(new GetRoleByIdQuery(Id))
                .MapResult();

            return response;
        })
        .WithTags(Tags.Role)
        .RequireAuthorization();
    }
}

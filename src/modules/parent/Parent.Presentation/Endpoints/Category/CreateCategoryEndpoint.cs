using Parent.Application.Features.Category;
using Parent.Presentation.Common;

namespace Parent.Presentation.Endpoints.Category;

internal sealed class CreateCategoryEndpoint(ICommandDispatcher _sender) : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("category", async (CreateCategoryCommand request) =>
        {
            var response = await _sender
                .Dispatch<CreateCategoryCommand, Result>(request)
                .MapResult();

            return response;
        })
        .WithTags(Tags.Category)
        .RequireAuthorization();
    }
}
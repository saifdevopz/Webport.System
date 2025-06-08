﻿using Common.Application.Messaging;
using Parent.Application.Features.Category;
using Parent.Presentation.Common;

namespace Parent.Presentation.Endpoints.Category;

internal sealed class CreateCategoryEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("category", async (
            CreateCategoryCommand request,
            ICommandHandler<CreateCategoryCommand> handler,
            CancellationToken cancellationToken) =>
        {
            var response = await handler.Handle(request, cancellationToken)
                                        .MapResult();

            return response;
        })
        .WithTags(Tags.Category)
        .RequireAuthorization();
    }
}


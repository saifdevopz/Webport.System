﻿using Tenant.Application.Features.INItem;

namespace Tenant.Presentation.Endpoints.INItem;

internal sealed class DeleteItemEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapDelete("item/{Id}", async (
            int Id,
            ICommandHandler<DeleteItemCommand> handler,
            CancellationToken cancellationToken) =>
        {
            var response = await handler
                .Handle(new DeleteItemCommand(Id), cancellationToken)
                .MapResult();

            return response;
        })
        .WithTags(Tags.Item)
        .RequireAuthorization();
    }
}
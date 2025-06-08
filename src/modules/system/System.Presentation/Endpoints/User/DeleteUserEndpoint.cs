﻿using System.Application.Features.Users;

namespace System.Presentation.Endpoints.User;

internal sealed class DeleteUserEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapDelete("user/{Id}", async (
            int Id,
            ICommandHandler<DeleteUserCommand> handler,
            CancellationToken cancellationToken) =>
        {
            var response = await handler
                .Handle(new DeleteUserCommand(Id), cancellationToken)
                .MapResult();

            return response;
        })
        .WithTags(Tags.User);
    }
}
﻿using System.Application.Features.Token;

namespace System.Presentation.Endpoints.Token;

internal sealed class AccessTokenEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("access", async (
            AccessTokenCommand request,
            ICommandHandler<AccessTokenCommand, AccessTokenResult> handler,
            CancellationToken cancellationToken) =>
        {
            var response = await handler
                .Handle(request, cancellationToken)
                .MapResult();

            return response;
        })
        .WithTags(Tags.Token);
    }
}

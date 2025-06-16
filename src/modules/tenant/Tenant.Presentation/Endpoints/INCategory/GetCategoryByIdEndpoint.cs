﻿using Tenant.Application.Features.INCategory;

namespace Tenant.Presentation.Endpoints.INCategory;

internal sealed class GetCategoryByIdEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("category/{Id}", async (
            int Id,
            IQueryHandler<GetCategoryByIdQuery, GetCategoryByIdQueryResult> handler,
            CancellationToken cancellationToken) =>
        {
            var response = await handler
                .Handle(new GetCategoryByIdQuery(Id), cancellationToken)
                .MapResult();

            return response;
        })
        .WithTags(Tags.Category)
        .RequireAuthorization();
    }
}

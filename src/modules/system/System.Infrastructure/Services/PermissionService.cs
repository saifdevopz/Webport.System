using Common.Application.Authorization;
using Common.Application.Messaging;
using System.Application.Features.Permissions;

namespace System.Infrastructure.Services;

internal sealed class PermissionService(
    IQueryHandler<GetPermissionsByUserIdQuery, GetPermissionsByUserIdQueryResult> handler)
    : IPermissionService
{
    public async Task<PermissionsResponse> GetUserPermissionsAsync(int userId)
    {
        var response = await handler
            .Handle(new GetPermissionsByUserIdQuery(userId), default);

        return response.Data.Permissions;
    }
}
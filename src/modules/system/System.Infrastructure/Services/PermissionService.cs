using Common.Application.Authorization;
using Common.Application.Messaging;
using Common.Domain.Results;
using System.Application.Features.Permissions;

namespace System.Infrastructure.Services;

internal sealed class PermissionService(
    IQueryHandler<GetPermissionsByUserIdQuery, GetPermissionsByUserIdQueryResult> handler)
    : IPermissionService
{
    public async Task<Result<PermissionsResponse>> GetUserPermissionsAsync(int userId)
    {
        var response = await handler
            .Handle(new GetPermissionsByUserIdQuery(userId), default);

        return Result.Success(response.Data.Permissions);
    }
}
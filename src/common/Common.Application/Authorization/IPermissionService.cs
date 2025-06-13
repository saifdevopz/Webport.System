namespace Common.Application.Authorization;

public interface IPermissionService
{
    Task<PermissionsResponse> GetUserPermissionsAsync(int userId);
}
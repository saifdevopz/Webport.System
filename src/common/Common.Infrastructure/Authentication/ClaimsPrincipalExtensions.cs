using System.Security.Claims;

namespace Common.Infrastructure.Authentication;

public static class ClaimsPrincipalExtensions
{
    public static string GetTenantDbName(this ClaimsPrincipal? principal)
    {
        return principal?.FindFirst(CustomClaims.DatabaseName)?.Value ?? string.Empty;
    }

    public static string GetUserEmail(this ClaimsPrincipal? principal)
    {
        return principal?.FindFirst(ClaimTypes.Email)?.Value ??
                     throw new CustomException("User Email is unavailable");
    }

    public static int GetUserId(this ClaimsPrincipal? principal)
    {
        string? userId = principal?.FindFirst(CustomClaims.Sub)?.Value;

        return int.TryParse(userId, out int parsedUserId) ?
        parsedUserId :
            throw new CustomException("User identifier is unavailable");
    }

    public static HashSet<string> GetPermissions(this ClaimsPrincipal? principal)
    {
        IEnumerable<Claim> permissionClaims = principal?.FindAll(CustomClaims.Permission) ??
                                              throw new CustomException("Permissions are unavailable");

        return [.. permissionClaims.Select(c => c.Value)];
    }
}

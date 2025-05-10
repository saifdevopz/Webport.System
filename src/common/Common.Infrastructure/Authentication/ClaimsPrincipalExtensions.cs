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
}

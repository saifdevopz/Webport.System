using Blazor.Common.Dtos;

namespace Blazor.Server.Services.Interfaces;

public interface ITokenService
{
    Task<TokenResponse?> AccessToken(LoginDto request, CancellationToken cancellationToken = default);
    Task<TokenResponse> RefreshToken(RefreshTokenRequest request, CancellationToken cancellationToken = default);
}
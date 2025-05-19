using Common.Domain.DataTransferObjects.System;
using Common.Domain.Results;

namespace Blazor.Server.Services.Interfaces;

public interface ITokenService
{
    Task<Result<TokenResponse>> AccessToken(LoginDto request, CancellationToken cancellationToken = default);
    Task<TokenResponse> RefreshToken(RefreshTokenRequest request, CancellationToken cancellationToken = default);
}
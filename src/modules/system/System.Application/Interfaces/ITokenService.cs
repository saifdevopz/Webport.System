using Common.Domain.Results;
using System.Application.Dtos;

namespace System.Application.Interfaces;

public interface ITokenService
{
    Task<Result<TokenResponse>> AccessToken(AccessTokenRequest request);
    Task<Result<TokenResponse>> RefreshToken(RefreshTokenRequest request);
}

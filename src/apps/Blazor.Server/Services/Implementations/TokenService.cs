using Blazor.Server.Common.Dtos;
using Blazor.Server.HttpClients;
using Blazor.Server.Services.Interfaces;
using Common.Domain.Results;


namespace Blazor.Server.Services.Implementations;
public class TokenService(BaseHttpClient httpClient) : ITokenService
{
    public const string baseUrl = "/token/accesstoken";

    public async Task<Result<TokenResponse>> AccessToken(LoginDto request, CancellationToken cancellationToken = default)
    {
        HttpClient httpclient = httpClient.GetPublicHttpClient();

        HttpResponseMessage response = await httpclient.PostAsJsonAsync($"/access", request, cancellationToken);
        var result = await response.Content.ReadFromJsonAsync<Result<TokenResponse>>(cancellationToken);

        return Result.Success(result!.Data);
    }

    public Task<TokenResponse> RefreshToken(RefreshTokenRequest request, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}

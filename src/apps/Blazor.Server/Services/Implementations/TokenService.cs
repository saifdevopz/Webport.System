using Blazor.Common.Dtos;
using Blazor.Common.Results;
using Blazor.Server.HttpClients;
using Blazor.Server.Services.Interfaces;

namespace Blazor.Server.Services.Implementations;
public class TokenService(BaseHttpClient httpClient) : ITokenService
{
    public const string baseUrl = "/token/accesstoken";

    public async Task<TokenResponse?> AccessToken(LoginDto request, CancellationToken cancellationToken = default)
    {
        HttpClient httpclient = httpClient.GetPublicHttpClient();

        HttpResponseMessage response = await httpclient.PostAsJsonAsync($"/access", request, cancellationToken);
        var result = await response.Content.ReadFromJsonAsync<ApiResult<TokenResponse>>(cancellationToken);

        return result!.Value;
    }

    public Task<TokenResponse> RefreshToken(RefreshTokenRequest request, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}

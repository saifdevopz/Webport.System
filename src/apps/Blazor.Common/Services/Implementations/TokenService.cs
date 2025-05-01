using Blazor.Common.Dtos;
using Blazor.Common.HttpClients;
using Blazor.Common.Results;
using Blazor.Common.Services.Interfaces;
using System.Net.Http.Json;

namespace Blazor.Common.Services.Implementations;
public class TokenService(BaseHttpClient httpClient) : ITokenService
{
    public const string baseUrl = "/token/accesstoken";

    public async Task<TokenResponse?> AccessToken(LoginDto request, CancellationToken cancellationToken = default)
    {
        HttpClient httpclient = httpClient.GetPublicHttpClient();

        HttpResponseMessage response = await httpclient.PostAsJsonAsync($"{baseUrl}/refresh", request, cancellationToken);
        var result = await response.Content.ReadFromJsonAsync<ApiResult<TokenResponse>>(cancellationToken);

        return result!.Data;
    }

    public Task<TokenResponse> RefreshToken(RefreshTokenRequest request, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}

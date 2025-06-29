using Blazor.Server.Common.HttpClients;
using Blazor.Server.Services.Interfaces;
using Common.Domain.DataTransferObjects.System;
using Common.Domain.Errors;
using Common.Domain.Results;
using Microsoft.AspNetCore.Mvc;


namespace Blazor.Server.Services.Implementations;
public class TokenService(BaseHttpClient httpClient) : ITokenService
{
    public const string baseUrl = "/token/accesstoken";

    public async Task<Result<TokenResponse>> AccessToken(LoginDto request, CancellationToken cancellationToken = default)
    {
        try
        {
            HttpClient httpclient = httpClient.GetPublicHttpClient();
            var response = await httpclient.PostAsJsonAsync($"/access", request, cancellationToken);

            if (response == null)
                return Result.Failure<TokenResponse>(CustomError.Conflict("NETWORK", "No response from server."));

            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<Result<TokenResponse>>(cancellationToken);

                if (result == null || result.IsFailure)
                    return Result.Failure<TokenResponse>(CustomError.Conflict("RESPONSE", "Unexpected empty or failed result."));

                return Result.Success(result.Data);
            }

            var problem = await response.Content.ReadFromJsonAsync<ProblemDetails>(cancellationToken);

            if (problem != null)
            {
                return Result.Failure<TokenResponse>(CustomError.Failure(problem.Title!, problem.Detail!));
            }

            return Result.Failure<TokenResponse>(CustomError.Conflict("HTTP", $"Unexpected error: {response.StatusCode}"));
        }
        catch (HttpRequestException ex)
        {
            return Result.Failure<TokenResponse>(CustomError.Conflict(",", $"{ex.Message}"));
        }
    }

    public Task<TokenResponse> RefreshToken(RefreshTokenRequest request, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}

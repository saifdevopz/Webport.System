using Common.Domain.Results;
using Common.Domain.System;
using System.Net.Http.Json;

namespace Parent.Application.Interfaces;

public interface ISystemService
{
    Task<ApiResponse<GetAllTenantsDto>> GetAllTenantsAsync(CancellationToken cancellation = default);
}

public class SystemService(HttpClient httpClient) : ISystemService
{
    public async Task<ApiResponse<GetAllTenantsDto>> GetAllTenantsAsync(CancellationToken cancellation = default)
    {
#pragma warning disable CA2234 // Pass system uri objects instead of strings
        var response = await httpClient.GetAsync("/tenant", cancellation);
#pragma warning restore CA2234 // Pass system uri objects instead of strings
        response.EnsureSuccessStatusCode();

        var tenants = await response.Content.ReadFromJsonAsync<ApiResponse<GetAllTenantsDto>>(cancellationToken: cancellation);
        return tenants!;
    }

}
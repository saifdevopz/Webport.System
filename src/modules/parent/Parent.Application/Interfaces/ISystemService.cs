using Common.Domain.DataTransferObjects.System;
using Common.Domain.Results;
using System.Net.Http.Json;

namespace Parent.Application.Interfaces;

public interface ISystemService
{
    Task<ApiResponse<TenantWrapper<GetTenantDto>>> GetAllTenantsAsync(CancellationToken cancellation = default);
}

public class SystemService(HttpClient httpClient) : ISystemService
{
    public async Task<ApiResponse<TenantWrapper<GetTenantDto>>> GetAllTenantsAsync(CancellationToken cancellation = default)
    {
#pragma warning disable CA2234 // Pass system uri objects instead of strings
        var response = await httpClient.GetAsync("/tenant", cancellation);
#pragma warning restore CA2234 // Pass system uri objects instead of strings
        response.EnsureSuccessStatusCode();

        var tenants = await response.Content.ReadFromJsonAsync<ApiResponse<TenantWrapper<GetTenantDto>>>(cancellationToken: cancellation);
        return tenants!;
    }

}
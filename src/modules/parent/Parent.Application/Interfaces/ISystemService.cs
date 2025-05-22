using Common.Domain.DataTransferObjects.System;
using Common.Domain.Results;
using System.Net.Http.Json;

namespace Parent.Application.Interfaces;

public interface ISystemService
{
    Task<Result<TenantsWrapper<GetTenantDto>>> GetAllTenantsAsync(CancellationToken cancellation = default);
}

public class SystemService(HttpClient httpClient) : ISystemService
{
    public async Task<Result<TenantsWrapper<GetTenantDto>>> GetAllTenantsAsync(CancellationToken cancellation = default)
    {
#pragma warning disable CA2234 // Pass system uri objects instead of strings
        var response = await httpClient.GetAsync("/tenant", cancellation);
#pragma warning restore CA2234 // Pass system uri objects instead of strings
        response.EnsureSuccessStatusCode();

        var result = await response.Content.ReadFromJsonAsync<Result<TenantsWrapper<GetTenantDto>>>(cancellationToken: cancellation);
        return result!;
    }

}
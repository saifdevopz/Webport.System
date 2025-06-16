using Common.Domain.DataTransferObjects.System;
using System.Net.Http.Json;

namespace Tenant.Application.Interfaces;

public interface ISystemService
{
    Task<Result<TenantsWrapper<GetTenantDto>>> GetAllTenantsAsync();
}

public class SystemService(HttpClient httpClient) : ISystemService
{
    public async Task<Result<TenantsWrapper<GetTenantDto>>> GetAllTenantsAsync()
    {
#pragma warning disable CA2234 // Pass system uri objects instead of strings
        var response = await httpClient.GetAsync("/tenant");
#pragma warning restore CA2234 // Pass system uri objects instead of strings
        response.EnsureSuccessStatusCode();

        var result = await response.Content.ReadFromJsonAsync<Result<TenantsWrapper<GetTenantDto>>>();
        return result!;
    }

}
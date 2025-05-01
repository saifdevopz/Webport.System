using System.Domain.Models;

namespace System.Domain.DTOs.Tenants;

public static class TenantMappings
{
    public static TenantM ToEntity(this CreateTenantDto dto)
    {
        TenantM result = new()
        {
            TenantName = dto.TenantName!,
            DatabaseName = dto.DatabaseName!,
            ConnectionString = "Connection - " + dto.DatabaseName,
            LicenceExpiryDate = DateTime.Now.AddDays(30),
        };

        return result;
    }

    public static TenantDto ToDto(this TenantM entity)
    {
        TenantDto result = new()
        {
            TenantName = entity.TenantName!,
            DatabaseName = entity.DatabaseName!
        };

        return result;
    }
}

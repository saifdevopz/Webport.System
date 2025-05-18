namespace Common.Domain.DataTransferObjects.System;

public class GetTenantDto
{
    public int TenantId { get; set; }
    public string TenantName { get; set; } = string.Empty;
    public string DatabaseName { get; set; } = string.Empty;
    public string ConnectionString { get; set; } = string.Empty;
    public DateTime LicenceExpiryDate { get; set; }
}


public record TenantWrapper<T>(IEnumerable<T> Tenants);
public record TenantWrap<T>(T Tenant);
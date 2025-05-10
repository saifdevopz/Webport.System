namespace Common.Domain.System;

public class GetAllTenantsDto
{
    public IEnumerable<TenantDto>? tenants { get; set; }
}

public class TenantDto
{
    public int TenantId { get; set; }
    public string? TenantName { get; set; }
    public string? DatabaseName { get; set; }
    public string? ConnectionString { get; set; }
    public DateTime LicenceExpiryDate { get; set; }
    public string? LastModBy { get; set; }
    public DateTime LastModDt { get; set; }
    public string? CreatedBy { get; set; }
    public DateTime CreatedDt { get; set; }
    public bool IsActive { get; set; }

}
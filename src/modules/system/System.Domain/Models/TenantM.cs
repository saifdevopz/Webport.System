namespace System.Domain.Models;

public sealed class TenantM : AggregateRoot
{
    public int TenantId { get; set; }
    public string TenantName { get; set; } = string.Empty;
    public string DatabaseName { get; set; } = string.Empty;
    public string ConnectionString { get; set; } = string.Empty;
    public DateTime LicenceExpiryDate { get; set; }

    public static TenantM Create(
        string tenantName,
        string databaseName)
    {
        TenantM tenant = new()
        {
            TenantName = tenantName,
            DatabaseName = databaseName,
            LicenceExpiryDate = DateTime.Now.AddDays(7),
        };

        return tenant;
    }
}


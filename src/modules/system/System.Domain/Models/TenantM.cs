namespace System.Domain.Models;

public sealed class TenantM : AggregateRoot
{
    public int TenantId { get; set; }
    public required string TenantName { get; set; }
    public required string DatabaseName { get; set; }
    public required string ConnectionString { get; set; }
    public DateTime LicenceExpiryDate { get; set; }

    public static TenantM Create(
        string tenantName,
        string databaseName)
    {
        TenantM obj = new()
        {
            TenantName = tenantName,
            DatabaseName = databaseName,
            ConnectionString = GeneralExtensions.BuildConnectionString(databaseName),
            LicenceExpiryDate = DateTime.Now.AddDays(30),
        };

        return obj;
    }


}


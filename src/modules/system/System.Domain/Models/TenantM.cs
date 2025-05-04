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
            ConnectionString = BuildConnectionString(databaseName),
            LicenceExpiryDate = DateTime.Now.AddDays(30),
        };

        return obj;
    }

    private static string BuildConnectionString(string databaseName)
    {
        return $"Host=102.211.206.231;Port=5432;Database={databaseName};Username=sa;Password=25122000SK;Pooling=true;MinPoolSize=10;MaxPoolSize=100;Include Error Detail=true";
    }
}


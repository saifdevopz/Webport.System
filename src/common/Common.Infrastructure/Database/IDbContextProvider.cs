using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Common.Infrastructure.Database;

public interface IDbContextProvider
{
    DbContext GetContext();
}

public class DbContextProvider(IServiceScopeFactory serviceFactory, string assemblyName) : IDbContextProvider
{
    private readonly IServiceScopeFactory _serviceFactory = serviceFactory ?? throw new ArgumentNullException(nameof(serviceFactory));
    private readonly string _assemblyName = assemblyName ?? throw new ArgumentNullException(nameof(assemblyName));

    public DbContext GetContext()
    {
        var scope = _serviceFactory.CreateScope();

        // Dynamically split the assembly name and remove any part after the first `.`
        string rootAssemblyName = _assemblyName.Split('.')[0];

        string dbContextTypeName = $"{rootAssemblyName}DbContext";

        Type? dbContextType = AppDomain.CurrentDomain
            .GetAssemblies() // Get all loaded assemblies
            .SelectMany(a => a.GetTypes()) // Get all types in each assembly
            .FirstOrDefault(t => t.Name == dbContextTypeName && typeof(DbContext).IsAssignableFrom(t));

        return dbContextType == null
            ? throw new InvalidOperationException("Unable to determine the calling assembly.")
            : (DbContext)scope.ServiceProvider.GetRequiredService(dbContextType);
    }

}
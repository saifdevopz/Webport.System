using Common.Infrastructure.Database;
using Common.Presentation.Endpoints;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Parent.Infrastructure.Database;
using System.Reflection;

namespace Parent.Infrastructure;

public static class ParentModule
{
    public static IServiceCollection AddParentModule(
        this IServiceCollection services,
        IConfiguration configuration,
        string parentDatabaseString)
    {
        ArgumentNullException.ThrowIfNull(configuration);

        services.AddInfrastructure(parentDatabaseString);

        services.AddEndpoints(Presentation.AssemblyReference.Assembly);

        return services;
    }

    private static void AddInfrastructure(
        this IServiceCollection services,
        string parentDatabaseString)
    {
        string AssemblyName = Assembly.GetCallingAssembly().GetName().Name!;
        services.AddScoped<IDbContextProvider>(sp =>
        {
            var scopeFactory = sp.GetRequiredService<IServiceScopeFactory>();
            return new DbContextProvider(scopeFactory, AssemblyName);
        });

        services.AddDbContext<ParentDbContext>((sp, options) =>
        {
            options.UseNpgsql(parentDatabaseString, npgsqlOptionsAction =>
            {
                npgsqlOptionsAction.EnableRetryOnFailure(
                        maxRetryCount: 5,
                        maxRetryDelay: TimeSpan.FromSeconds(2),
                        errorCodesToAdd: null);

                npgsqlOptionsAction.MigrationsHistoryTable(HistoryRepository.DefaultTableName, ParentConstants.Schema);
            });
        });
    }
}
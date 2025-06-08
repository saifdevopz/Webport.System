using Common.Application.Database;
using Common.Infrastructure.Authentication;
using Common.Infrastructure.Database;
using Common.Presentation.Endpoints;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Parent.Application.Interfaces;
using Parent.Infrastructure.Database;
using Parent.Infrastructure.Services;
using System.Reflection;

namespace Parent.Infrastructure;

public static class ParentModule
{
#pragma warning disable S1075 // URIs should not be hardcoded
    private const string UriString = "https://system.webport.co.za";
#pragma warning restore S1075 // URIs should not be hardcoded

    public static IServiceCollection AddParentModule(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        ArgumentNullException.ThrowIfNull(configuration);

        services.AddInfrastructure();

        services.AddEndpoints(Presentation.AssemblyReference.Assembly);

        return services;
    }

    private static void AddInfrastructure(
        this IServiceCollection services)
    {
        services.AddAuthenticationInternal();
        services.AddScoped<CurrentTenant>();

        services.AddScoped(sp =>
        {
            var client = new HttpClient
            {
                BaseAddress = new Uri(UriString)
            };
            return client;
        });

        services.AddScoped<ISystemService, SystemService>();

        string AssemblyName = Assembly.GetCallingAssembly().GetName().Name!;
        services.AddScoped<IDbContextProvider>(sp =>
        {
            var scopeFactory = sp.GetRequiredService<IServiceScopeFactory>();
            return new DbContextProvider(scopeFactory, AssemblyName);
        });

        services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));

        services.AddDbContext<ParentDbContext>((sp, options) =>
        {
            CurrentTenant tenantProvider = sp.GetRequiredService<CurrentTenant>();
            string parentDatabaseString = tenantProvider.GetTenantConnectionString();

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
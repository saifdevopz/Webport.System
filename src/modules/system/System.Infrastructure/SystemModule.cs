using Common.Infrastructure.Authentication;
using Common.Infrastructure.Database;
using Common.Presentation.Endpoints;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Application.Interfaces;
using System.Infrastructure.Database;
using System.Infrastructure.Database.Repository;
using System.Infrastructure.Services;
using System.Reflection;

namespace System.Infrastructure;

public static class SystemModule
{
    public static IServiceCollection AddSystemModule(
        this IServiceCollection services,
        IConfiguration configuration,
        string systemDatabaseString)
    {
        ArgumentNullException.ThrowIfNull(configuration);

        services.AddInfrastructure(systemDatabaseString);

        services.AddEndpoints(Presentation.AssemblyReference.Assembly);

        return services;
    }

    private static void AddInfrastructure(
        this IServiceCollection services,
        string systemDatabaseString)
    {
        string AssemblyName = Assembly.GetCallingAssembly().GetName().Name!;
        services.AddScoped<IDbContextProvider>(sp =>
        {
            var scopeFactory = sp.GetRequiredService<IServiceScopeFactory>();
            return new DbContextProvider(scopeFactory, AssemblyName);
        });

        services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

        services.AddScoped<ITokenService, TokenService>();

        services.AddAuthenticationInternal();

        services.AddDbContext<SystemDbContext>((sp, options) =>
        {
            options.UseSqlServer(systemDatabaseString, sqlServerOptionsAction: sqlOptions =>
            {
                sqlOptions.EnableRetryOnFailure(
                maxRetryCount: 10,
                maxRetryDelay: TimeSpan.FromSeconds(5),
                errorNumbersToAdd: null);
            });
        });
    }
}
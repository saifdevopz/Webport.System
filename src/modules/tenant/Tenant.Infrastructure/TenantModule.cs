using Common.Application.Database;
using Common.Application.Messaging;
using Common.Infrastructure.Database;
using Common.Presentation.Endpoints;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System.Reflection;
using Tenant.Application.Interfaces;
using Tenant.Infrastructure.Database;
using Tenant.Infrastructure.Outbox;

namespace Tenant.Infrastructure;

public static class TenantModule
{
    public static IServiceCollection AddTenantModule(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        ArgumentNullException.ThrowIfNull(configuration);

        services.AddDomainEventHandlers();

        services.AddInfrastructure(configuration);

        services.AddEndpoints(Presentation.AssemblyReference.Assembly);

        return services;
    }

    private static void AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        string AssemblyName = Assembly.GetCallingAssembly().GetName().Name!;
        services.AddScoped<IDbContextProvider>(sp =>
        {
            var scopeFactory = sp.GetRequiredService<IServiceScopeFactory>();
            return new DbContextProvider(scopeFactory, AssemblyName);
        });

        services.AddScoped<ISystemService, SystemService>();
        services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));

        services.AddScoped(sp =>
        {
            var client = new HttpClient
            {
                BaseAddress = new Uri(configuration.GetSection("Urls:SystemBaseUrl").Value!)
            };

            return client;
        });

        services.AddDbContext<TenantDbContext>((sp, options) =>
        {
            CurrentConnection currentConnection = sp.GetRequiredService<CurrentConnection>();
            string parentDatabaseString = currentConnection.GetParentConnectionString();

            options.UseNpgsql(parentDatabaseString, npgsqlOptionsAction =>
            {
                npgsqlOptionsAction.EnableRetryOnFailure(
                        maxRetryCount: 5,
                        maxRetryDelay: TimeSpan.FromSeconds(2),
                        errorCodesToAdd: null);

                npgsqlOptionsAction.MigrationsHistoryTable(HistoryRepository.DefaultTableName, ParentConstants.Schema);
            })
            .UseSnakeCaseNamingConvention();
        });

        services.Configure<OutboxOptions>(configuration.GetSection("Events:Outbox"));
        services.ConfigureOptions<ConfigureProcessOutboxJob>();
    }

    private static void AddDomainEventHandlers(this IServiceCollection services)
    {
        Type[] domainEventHandlers = [.. Application.AssemblyReference.Assembly
            .GetTypes()
            .Where(t => t.IsAssignableTo(typeof(IDomainEventDispatcher)))];

        foreach (Type domainEventHandler in domainEventHandlers)
        {
            services.TryAddScoped(domainEventHandler);

            Type domainEvent = domainEventHandler
                .GetInterfaces()
                .Single(i => i.IsGenericType)
                .GetGenericArguments()
                .Single();

            Type closedIdempotentHandler = typeof(IdempotentDomainEventHandler<>).MakeGenericType(domainEvent);

            services.Decorate(domainEventHandler, closedIdempotentHandler);
        }
    }
}
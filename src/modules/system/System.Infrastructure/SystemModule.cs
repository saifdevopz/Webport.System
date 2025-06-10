using Common.Application.Database;
using Common.Application.Messaging;
using Common.Infrastructure.Authentication;
using Common.Infrastructure.Database;
using Common.Infrastructure.Interceptors;
using Common.Presentation.Endpoints;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System.Application.Interfaces;
using System.Infrastructure.Database;
using System.Infrastructure.Outbox;
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

        services.AddDomainEventHandlers();

        services.AddInfrastructure(configuration, systemDatabaseString);

        services.AddEndpoints(Presentation.AssemblyReference.Assembly);

        return services;
    }

    private static void AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration,
        string systemDatabaseString)
    {
        string AssemblyName = Assembly.GetCallingAssembly().GetName().Name!;
        services.AddScoped<IDbContextProvider>(sp =>
        {
            var scopeFactory = sp.GetRequiredService<IServiceScopeFactory>();
            return new DbContextProvider(scopeFactory, AssemblyName);
        });

        services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));

        services.AddScoped<ITokenService, TokenService>();

        services.AddAuthenticationInternal();

        services.AddScoped<DataSeeder>();

        services.AddDbContext<SystemDbContext>((sp, options) =>
        {
            options.UseNpgsql(systemDatabaseString, npgsqlOptionsAction =>
            {
                npgsqlOptionsAction.EnableRetryOnFailure(
                        maxRetryCount: 5,
                        maxRetryDelay: TimeSpan.FromSeconds(2),
                        errorCodesToAdd: null);

                npgsqlOptionsAction.MigrationsHistoryTable(HistoryRepository.DefaultTableName, SystemConstants.Schema);
            })
            .UseSnakeCaseNamingConvention()
            .AddInterceptors(sp.GetRequiredService<InsertOutboxMessagesInterceptor>());
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
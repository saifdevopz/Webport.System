using Common.Application.Behaviors;
using Common.Application.CQRS;
using Common.Infrastructure.CQRS;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace Common.Infrastructure;

public static class InfrastructureConfiguration
{
    public static IServiceCollection AddCommonInfrastructure(
        this IServiceCollection services)
    {
        services.AddScoped<ICommandDispatcher, CommandDispatcher>();
        services.AddScoped<IQueryDispatcher, QueryDispatcher>();

        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>));
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

        RegisterQueryHandlers(services);
        RegisterCommandHandlers(services);

        return services;
    }

    private static void RegisterQueryHandlers(IServiceCollection services)
    {
        foreach (var type in AppDomain.CurrentDomain.GetAssemblies()
            .SelectMany(s => s.GetTypes())
            .Where(p => p.IsClass && !p.IsAbstract && p.GetInterfaces()
            .Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IQueryHandler<,>))))
        {
            foreach (var interfaceType in type.GetInterfaces()
                .Where(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IQueryHandler<,>)))
            {
                services.AddScoped(interfaceType, type);
            }
        }
    }

    private static void RegisterCommandHandlers(IServiceCollection services)
    {
        foreach (var type in AppDomain.CurrentDomain.GetAssemblies()
            .SelectMany(s => s.GetTypes())
            .Where(p => p.IsClass && !p.IsAbstract && p.GetInterfaces()
            .Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(ICommandHandler<,>))))
        {
            foreach (var interfaceType in type.GetInterfaces()
                .Where(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(ICommandHandler<,>)))
            {
                services.AddScoped(interfaceType, type);
            }
        }
    }
}
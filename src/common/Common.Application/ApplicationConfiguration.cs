using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Common.Application;

public static class ApplicationConfiguration
{
    public static IServiceCollection AddCommonApplication(
        this IServiceCollection services,
        Assembly[] moduleAssemblies)
    {
        services.AddValidatorsFromAssemblies(moduleAssemblies, includeInternalTypes: true);

        return services;
    }
}


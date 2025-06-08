using Microsoft.Extensions.DependencyInjection;

namespace Common.Infrastructure;

public static class InfrastructureConfiguration
{
    public static IServiceCollection AddCommonInfrastructure(
        this IServiceCollection services)
    {
        return services;
    }
}
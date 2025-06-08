using Microsoft.Extensions.Configuration;

namespace Common.Infrastructure.Extensions;

public static class ConfigurationExtensions
{
    public static T GetValueOrThrow<T>(this IConfiguration configuration, string name)
    {
        return configuration.GetValue<T?>(name) ??
                     throw new InvalidOperationException($"The connection string {name} was not found");
    }
}
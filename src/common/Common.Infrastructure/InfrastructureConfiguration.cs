using Common.Application.Database;
using Common.Infrastructure.Authentication;
using Common.Infrastructure.Clock;
using Common.Infrastructure.Database;
using Common.Infrastructure.Interceptors;
using Common.Infrastructure.Mail;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Quartz;

namespace Common.Infrastructure;

public static class InfrastructureConfiguration
{
    public static IServiceCollection AddCommonInfrastructure(
        this IServiceCollection services)
    {
        // Mail
        services.ConfigureMailing();

        services.AddAuthenticationInternal();

        services.TryAddSingleton<IDateTimeProvider, DateTimeProvider>();
        services.AddTransient<CurrentConnection>();
        services.TryAddSingleton<InsertOutboxMessagesInterceptor>();

        services.TryAddScoped<IDbConnectionFactory, DbConnectionFactory>();

        //Quartz
        services.AddQuartz(configurator =>
        {
            Guid scheduler = Guid.NewGuid();
            configurator.SchedulerId = $"default-id-{scheduler}";
            configurator.SchedulerName = $"default-name-{scheduler}";
        });

        services.AddQuartzHostedService(options => options.WaitForJobsToComplete = true);

        return services;
    }
}
using Common.Application.Database;
using Common.Infrastructure.Authentication;
using Common.Infrastructure.Clock;
using Common.Infrastructure.Database;
using Common.Infrastructure.Interceptors;
using Common.Infrastructure.Mail;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Npgsql;
using OpenTelemetry;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using Quartz;

namespace Common.Infrastructure;

public static class InfrastructureConfiguration
{
    public static IServiceCollection AddCommonInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration,
        string serviceName)
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

        // OpenTelemetry

        services.AddOpenTelemetry()
                .ConfigureResource(_ => _.AddService(serviceName))
                .WithMetrics(metrics =>
                {
                    metrics
                        .AddAspNetCoreInstrumentation()
                        .AddHttpClientInstrumentation()
                        .AddNpgsqlInstrumentation();
                    //.AddOtlpExporter(options =>
                    //{
                    //    //var otelConfig = configuration.GetSection("OpenTelemetry");

                    //    //options.Endpoint = new Uri(otelConfig["Endpoint"]!);
                    //    //options.Headers = otelConfig["Headers"]!;
                    //    //options.Protocol = OpenTelemetry.Exporter.OtlpExportProtocol.HttpProtobuf;
                    //});

                })
                .WithTracing(tracing =>
                {
                    tracing
                        .AddHttpClientInstrumentation()
                        .AddAspNetCoreInstrumentation()
                        .AddEntityFrameworkCoreInstrumentation()
                        .AddRedisInstrumentation()
                        .AddNpgsql();

                    //tracing.AddOtlpExporter(options =>
                    //    {
                    //        //var otelConfig = configuration.GetSection("OpenTelemetry");

                    //        //options.Endpoint = new Uri(otelConfig["Endpoint"]!);
                    //        //options.Headers = otelConfig["Headers"]!;
                    //        //options.Protocol = OpenTelemetry.Exporter.OtlpExportProtocol.HttpProtobuf;
                    //    });

                })
                .UseOtlpExporter();

        return services;
    }
}
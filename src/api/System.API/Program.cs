using Common.Application;
using Common.Infrastructure;
using Common.Presentation.Endpoints;
using Scalar.AspNetCore;
using Serilog;
using System.API.Extensions;
using System.API.Middlewares;
using System.Infrastructure;
using System.Reflection;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

// Build logger temporarily so we can log before app is built
Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .CreateLogger();

// Environment info logging
var environment = builder.Environment;

if (environment.IsDevelopment())
{
    Log.Information("Running in Development environment.");
}
else if (environment.IsProduction())
{
    Log.Information("Running in Production environment.");
}
else
{
    Log.Information("Running in {EnvironmentName} environment.", environment.EnvironmentName);
}

// Serilog
//builder.Host.UseSerilog((context, loggerConfig) => loggerConfig.ReadFrom.Configuration(context.Configuration));

builder.Logging.AddOpenTelemetry(logging =>
{
    logging.IncludeScopes = true;
    logging.IncludeFormattedMessage = true;

    //var otelConfig = builder.Configuration.GetSection("OpenTelemetry");

    //logging.AddOtlpExporter(options =>
    //{
    //    options.Endpoint = new Uri(otelConfig["Endpoint"]!);
    //    options.Headers = otelConfig["Headers"]!;
    //    options.Protocol = OpenTelemetry.Exporter.OtlpExportProtocol.HttpProtobuf;
    //});
});


// Connection Strings
string? systemDatabaseString = builder.Configuration["PostgreSQL:DefaultConnection"];
ArgumentException.ThrowIfNullOrWhiteSpace(systemDatabaseString);

// Controller Support
builder.Services.AddControllers();

// Minimal API Support
builder.Services.AddEndpointsApiExplorer();

// Open API
builder.Services.AddOpenApi();

// Global Exception Handling
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();

// Application Module Assemblies
Assembly[] moduleApplicationAssemblies =
[
    System.Application.AssemblyReference.Assembly,
];

// Common Application Module
builder.Services.AddCommonApplication(moduleApplicationAssemblies);

// Common Infrastructure Module
builder.Services.AddCommonInfrastructure(builder.Configuration, DiagnosticsConfig.ServiceName);

builder.Services.AddSystemModule(builder.Configuration, systemDatabaseString);

// Add CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("MyPolicy", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

WebApplication app = builder.Build();

app.UseCors("MyPolicy");

if (app.Environment.IsDevelopment() || app.Environment.IsProduction())
{
    app.MapOpenApi();
    app.MapScalarApiReference(_ =>
    {
        _.Servers = [];
        _.Theme = ScalarTheme.Kepler;
    });
    await app.ApplyAllMigrations();
}

//app.UseSerilogRequestLogging();

app.UseExceptionHandler();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.MapEndpoints();

await app.RunAsync();


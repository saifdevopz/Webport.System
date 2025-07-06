using Common.Application;
using Common.Infrastructure;
using Common.Infrastructure.Middlewares;
using Common.Infrastructure.OpenAPI;
using Common.Presentation.Endpoints;
using QuestPDF.Infrastructure;
using Scalar.AspNetCore;
using Serilog;
using System.Reflection;
using Tenant.API.Extensions;
using Tenant.Infrastructure;

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

// QuestPDF
QuestPDF.Settings.License = LicenseType.Community;

// Connection Strings
string? parentDatabaseString = builder.Configuration["PostgreSQL:DefaultConnection"];
ArgumentException.ThrowIfNullOrWhiteSpace(parentDatabaseString);

// Serilog
builder.Host.UseSerilog((context, loggerConfig) => loggerConfig.ReadFrom.Configuration(context.Configuration));

// Controller Support
builder.Services.AddControllers();

// Minimal API Support
builder.Services.AddEndpointsApiExplorer();

// Open API
builder.Services.AddOpenApi("v1", options =>
{
    options.AddDocumentTransformer<BearerSecuritySchemeTransformer>();
});

// Global Exception Handling
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();

// Application Module Assemblies
Assembly[] moduleApplicationAssemblies =
[
    Tenant.Application.AssemblyReference.Assembly,
];

// Common Application Module
builder.Services.AddCommonApplication(moduleApplicationAssemblies);

// Common Infrastructure Module
builder.Services.AddCommonInfrastructure(builder.Configuration, DiagnosticsConfig.ServiceName);

builder.Services.AddTenantModule(builder.Configuration);

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

var app = builder.Build();

app.UseCors("MyPolicy");

app.UseStaticFiles();

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

app.UseApplicationMiddlewares();

app.UseSerilogRequestLogging();

app.UseExceptionHandler();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.MapEndpoints();

await app.RunAsync();

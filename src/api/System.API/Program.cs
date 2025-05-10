using Common.Application;
using Common.Infrastructure;
using Common.Presentation.Endpoints;
using Parent.API.Middleware;
using Scalar.AspNetCore;
using Serilog;
using System.API.Extensions;
using System.Infrastructure;
using System.Reflection;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

// Connection Strings
string? systemDatabaseString = builder.Configuration["SQLServer:ProductionConnection"];
ArgumentException.ThrowIfNullOrWhiteSpace(systemDatabaseString);

// Serilog
builder.Host.UseSerilog((context, loggerConfig) => loggerConfig.ReadFrom.Configuration(context.Configuration));

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
builder.Services.AddCommonInfrastructure();

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

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference(_ =>
    {
        _.Servers = [];
        _.Theme = ScalarTheme.BluePlanet;
    });
    await app.ApplyAllMigrations();
}

app.UseSerilogRequestLogging();

app.UseExceptionHandler();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.MapEndpoints();

await app.RunAsync();


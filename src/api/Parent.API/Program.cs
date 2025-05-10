using Common.Application;
using Common.Infrastructure;
using Common.Infrastructure.Middlewares;
using Common.Presentation.Endpoints;
using Parent.API.Extensions;
using Parent.Infrastructure;
using Scalar.AspNetCore;
using Serilog;
using System.Reflection;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

// Connection Strings
string? parentDatabaseString = builder.Configuration["PostgreSQL:DefaultConnection"];
ArgumentException.ThrowIfNullOrWhiteSpace(parentDatabaseString);

// Controller Support
builder.Services.AddControllers();

// Minimal API Support
builder.Services.AddEndpointsApiExplorer();

// Open API
builder.Services.AddOpenApi();

// Global Exception Handling
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();

// Serilog
builder.Host.UseSerilog((context, loggerConfig) => loggerConfig.ReadFrom.Configuration(context.Configuration));

// Application Module Assemblies
Assembly[] moduleApplicationAssemblies =
[
    Parent.Application.AssemblyReference.Assembly,
];

// Common Application Module
builder.Services.AddCommonApplication(moduleApplicationAssemblies);

// Common Infrastructure Module
builder.Services.AddCommonInfrastructure();

builder.Services.AddParentModule(builder.Configuration);

var app = builder.Build();

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

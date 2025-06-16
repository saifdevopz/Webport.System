using Microsoft.EntityFrameworkCore;
using Tenant.Application.Interfaces;
using Tenant.Infrastructure.Database;

namespace Tenant.API.Extensions;

internal static class MigrationExtensions
{
    public static async Task ApplyAllMigrations(this IApplicationBuilder app)
    {
        await app.ApplyTenantMigrations();
    }

    public static async Task ApplyTenantMigrations(this IApplicationBuilder app)
    {
        using var scope = app.ApplicationServices.CreateScope();
        var systemService = scope.ServiceProvider.GetRequiredService<ISystemService>();
        var tenants = await systemService.GetAllTenantsAsync();

        foreach (var tenantConnectionString in tenants.Data.Tenants.Select(_ => _.ConnectionString))
        {
            try
            {
                await app.ApplyCustomMigration<TenantDbContext>(tenantConnectionString);
            }
            catch (DbUpdateException ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"EF update error while migrating tenant '{tenantConnectionString}': {ex.Message}");
                Console.ResetColor();
            }
            catch (InvalidOperationException ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"Configuration error during migration for tenant '{tenantConnectionString}': {ex.Message}");
                Console.ResetColor();
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.WriteLine($"Unexpected error for tenant '{tenantConnectionString}': {ex.Message}");
                Console.ResetColor();

                throw;
            }
        }

        await Task.CompletedTask;
    }

    private static async Task ApplyCustomMigration<TDbContext>(this IApplicationBuilder app, string? connectionString)
        where TDbContext : DbContext
    {
        using IServiceScope scope = app.ApplicationServices.CreateScope();
        using TDbContext context = scope.ServiceProvider.GetRequiredService<TDbContext>();

        if (connectionString != null)
        {
            context.Database.SetConnectionString(connectionString);
        }

        if (!await context.Database.CanConnectAsync() && (await context.Database.GetPendingMigrationsAsync()).Any())
        {
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine($"Applying Migrations for '{connectionString ?? "Default Database"}'.");
            Console.ResetColor();
            await context.Database.MigrateAsync();
        }
    }
}

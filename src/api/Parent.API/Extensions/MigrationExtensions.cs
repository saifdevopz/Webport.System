using Microsoft.EntityFrameworkCore;
using Parent.Application.Interfaces;
using Parent.Infrastructure.Database;

namespace Parent.API.Extensions;

internal static class MigrationExtensions
{
    public static async Task ApplyAllMigrations(this IApplicationBuilder app)
    {
        await app.ApplyParentMigrations();
    }

    public static async Task ApplyParentMigrations(this IApplicationBuilder app)
    {
        using var scope = app.ApplicationServices.CreateScope();
        var systemService = scope.ServiceProvider.GetRequiredService<ISystemService>();
        var tenants = await systemService.GetAllTenantsAsync();

        foreach (var tenant in tenants.Data.Tenants!)
        {
            app.ApplyCustomMigration<ParentDbContext>(tenant.ConnectionString);
        }

        await Task.CompletedTask;
    }

    private static void ApplyCustomMigration<TDbContext>(this IApplicationBuilder app, string? connectionString)
        where TDbContext : DbContext
    {
        using IServiceScope scope = app.ApplicationServices.CreateScope();
        using TDbContext context = scope.ServiceProvider.GetRequiredService<TDbContext>();

        if (connectionString != null)
        {
            context.Database.SetConnectionString(connectionString);
        }

        if (context.Database.GetPendingMigrations().Any())
        {
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine($"Applying Migrations for '{connectionString ?? "Default Database"}'.");
            Console.ResetColor();
            context.Database.Migrate();
        }
    }
}

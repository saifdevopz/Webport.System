using Microsoft.EntityFrameworkCore;
using Parent.Infrastructure.Database;

namespace System.API.Extensions;

internal static class MigrationExtensions
{
    public static async Task ApplyAllMigrations(this IApplicationBuilder app)
    {
        await app.ApplyParentMigrations();
    }

    public static async Task ApplyParentMigrations(this IApplicationBuilder app)
    {
        app.ApplyCustomMigration<ParentDbContext>(null);
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

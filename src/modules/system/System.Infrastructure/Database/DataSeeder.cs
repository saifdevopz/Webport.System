using Microsoft.EntityFrameworkCore;
using System.Domain.Models;

namespace System.Infrastructure.Database;

public class DataSeeder(SystemDbContext context)
{
    private readonly SystemDbContext _context = context ?? throw new ArgumentNullException(nameof(context));

    public async Task SeedAsync()
    {
        await SeedTenantsAsync();
        await SeedRolesAsync();
        await SeedUsersAsync();
    }

    private async Task SeedTenantsAsync()
    {
        if (await _context.Tenants.AnyAsync())
        {
            return;
        }

        TenantM[] tenants =
        [
            TenantM.Create("Webport", "webport-db"),
            TenantM.Create("Customer1", "customer1-db"),
            TenantM.Create("Customer2", "customer2-db"),
        ];

        await _context.Tenants.AddRangeAsync(tenants);
        await _context.SaveChangesAsync();
    }

    private async Task SeedRolesAsync()
    {
        if (await _context.Roles.AnyAsync())
        {
            return;
        }

        RoleM[] roles =
        [
            RoleM.Create("Admin"),
            RoleM.Create("Customer")
        ];

        await _context.Roles.AddRangeAsync(roles);
        await _context.SaveChangesAsync();
    }

    private async Task SeedUsersAsync()
    {
        if (await _context.Users.AnyAsync())
        {
            return;
        }

        UserM[] users =
        [
            UserM.Create(1, 1, "Saif Khan", "saif@gmail.com", "12345678"),
            UserM.Create(2, 2, "Itachi Uchiha", "itachi@gmail.com", "12345678"),
            UserM.Create(3, 2, "Naruto Uzumaki", "naruto@gmail.com", "12345678"),
        ];

        await _context.Users.AddRangeAsync(users);
        await _context.SaveChangesAsync();
    }
}
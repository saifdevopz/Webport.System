using Microsoft.EntityFrameworkCore;
using System.Domain.Entities.Roles;
using System.Domain.Entities.Tenants;
using System.Domain.Entities.Users;

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
            UserM.Create(1, 1, "Saif Khan", "admin@gmail.com", "12345678"),
            UserM.Create(2, 2, "Itachi Uchiha", "customer1@gmail.com", "12345678"),
            UserM.Create(3, 2, "Naruto Uzumaki", "customer@gmail.com", "12345678"),
        ];

        await _context.Users.AddRangeAsync(users);
        await _context.SaveChangesAsync();
    }
}
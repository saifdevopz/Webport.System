using Common.Infrastructure.Interceptors;
using Common.Infrastructure.Outbox;
using Microsoft.EntityFrameworkCore;
using System.Domain.Entities.Permissions;
using System.Domain.Entities.Roles;
using System.Domain.Entities.Tenants;
using System.Domain.Entities.Users;

namespace System.Infrastructure.Database;

public sealed class SystemDbContext(DbContextOptions<SystemDbContext> options) : DbContext(options)
{
    public DbSet<TenantM> Tenants => Set<TenantM>();
    public DbSet<UserM> Users => Set<UserM>();
    internal DbSet<RoleM> Roles => Set<RoleM>();
    internal DbSet<PermissionM> Permissions => Set<PermissionM>();
    internal DbSet<RolePermissionM> RolePermissions => Set<RolePermissionM>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        ArgumentNullException.ThrowIfNull(modelBuilder);

        // Schema
        modelBuilder.HasDefaultSchema(SystemConstants.Schema);

        // Configurations
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(SystemDbContext).Assembly);
        modelBuilder.ApplyConfiguration(new OutboxMessageConfiguration());
        modelBuilder.ApplyConfiguration(new OutboxMessageConsumerConfiguration());
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        ArgumentNullException.ThrowIfNull(optionsBuilder);

        // Interceptors
        optionsBuilder.AddInterceptors(new AuditableEntityInterceptor());
        optionsBuilder.AddInterceptors(new InsertOutboxMessagesInterceptor());
    }
}


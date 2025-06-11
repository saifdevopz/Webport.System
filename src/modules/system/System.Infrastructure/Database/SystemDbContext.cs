using Common.Infrastructure.Interceptors;
using Common.Infrastructure.Outbox;
using Microsoft.EntityFrameworkCore;
using System.Domain.Models;

namespace System.Infrastructure.Database;

public sealed class SystemDbContext(DbContextOptions<SystemDbContext> options) : DbContext(options)
{
    public DbSet<TenantM> Tenants => Set<TenantM>();
    public DbSet<UserM> Users => Set<UserM>();
    public DbSet<RoleM> Roles => Set<RoleM>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        ArgumentNullException.ThrowIfNull(modelBuilder);

        modelBuilder.HasDefaultSchema(SystemConstants.Schema);

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


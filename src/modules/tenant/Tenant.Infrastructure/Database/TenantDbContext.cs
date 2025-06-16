using Common.Infrastructure.Interceptors;
using Common.Infrastructure.Outbox;
using Microsoft.EntityFrameworkCore;
using Tenant.Domain.Entities.INCategory;
using Tenant.Domain.Entities.INItem;

namespace Tenant.Infrastructure.Database;

public sealed class TenantDbContext(DbContextOptions<TenantDbContext> options) : DbContext(options)
{
    internal DbSet<INCategoryM> Categories => Set<INCategoryM>();
    internal DbSet<INItemM> Items => Set<INItemM>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        ArgumentNullException.ThrowIfNull(modelBuilder);

        // Schema
        modelBuilder.HasDefaultSchema(ParentConstants.Schema);

        // Configurations
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(TenantDbContext).Assembly);
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


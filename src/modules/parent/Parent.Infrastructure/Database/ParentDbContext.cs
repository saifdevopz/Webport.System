using Common.Infrastructure.Interceptors;
using Microsoft.EntityFrameworkCore;
using Parent.Domain.Inventory.Category;

namespace Parent.Infrastructure.Database;

public sealed class ParentDbContext(DbContextOptions<ParentDbContext> options) : DbContext(options)
{
    public DbSet<CategoryM> Categories => Set<CategoryM>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        ArgumentNullException.ThrowIfNull(modelBuilder);

        modelBuilder.HasDefaultSchema(ParentConstants.Schema);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ParentDbContext).Assembly);
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        ArgumentNullException.ThrowIfNull(optionsBuilder);

        optionsBuilder.AddInterceptors(new AuditableEntityInterceptor());
    }
}


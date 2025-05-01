using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Domain.Models;

namespace System.Infrastructure.Database.Configurations;

public sealed class TenantConfiguration : IEntityTypeConfiguration<TenantM>
{
    public void Configure(EntityTypeBuilder<TenantM> builder)
    {
        builder.HasKey(t => t.TenantId);

        builder.Property(t => t.TenantName).IsRequired().HasMaxLength(50);

        builder.Property(t => t.DatabaseName).HasMaxLength(50);

        builder.HasIndex(t => new { t.TenantName }).IsUnique();
    }
}

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Domain.Entities.Tenants;

namespace System.Infrastructure.Database.Configurations;

public sealed class TenantConfig : IEntityTypeConfiguration<TenantM>
{
    public void Configure(EntityTypeBuilder<TenantM> builder)
    {
        builder
            .HasKey(_ => _.TenantId);

        builder.Property(_ => _.TenantName)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(_ => _.DatabaseName)
            .IsRequired()
            .HasMaxLength(20);

        builder.Property(_ => _.ConnectionString)
            .IsRequired();

        builder.Property(_ => _.LicenceExpiryDate)
            .IsRequired();

        builder.HasIndex(_ => new { _.TenantName })
            .IsUnique();
    }
}

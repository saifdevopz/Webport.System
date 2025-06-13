using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Domain.Entities.Roles;

namespace System.Infrastructure.Database.Configurations;

public sealed class RoleConfig : IEntityTypeConfiguration<RoleM>
{
    public void Configure(EntityTypeBuilder<RoleM> builder)
    {
        builder
            .HasKey(_ => _.RoleId);

        builder.Property(_ => _.RoleName)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(_ => _.NormalizedRoleName)
            .HasMaxLength(50);

        builder.HasIndex(_ => new { _.NormalizedRoleName })
            .IsUnique();
    }
}
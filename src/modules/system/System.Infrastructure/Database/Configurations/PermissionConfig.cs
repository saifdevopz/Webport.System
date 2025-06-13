using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Domain.Entities.Permissions;

namespace System.Infrastructure.Database.Configurations;

internal sealed class PermissionConfig : IEntityTypeConfiguration<PermissionM>
{
    public void Configure(EntityTypeBuilder<PermissionM> builder)
    {
        builder.HasKey(_ => _.PermissionId);

        builder.Property(_ => _.PermissionCode).HasMaxLength(20);
    }
}
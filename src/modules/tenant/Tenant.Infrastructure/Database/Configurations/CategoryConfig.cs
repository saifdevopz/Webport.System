using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Tenant.Domain.Entities.INCategory;

namespace Tenant.Infrastructure.Database.Configurations;

public sealed class CategoryConfig : IEntityTypeConfiguration<INCategoryM>
{
    public void Configure(EntityTypeBuilder<INCategoryM> builder)
    {
        builder.HasKey(_ => _.CategoryId);

        builder.HasIndex(_ => new { _.CategoryCode }).IsUnique();

        builder.Property(_ => _.CategoryCode).IsRequired().HasMaxLength(20);

        builder.Property(_ => _.CategoryDesc).IsRequired().HasMaxLength(50);
    }
}

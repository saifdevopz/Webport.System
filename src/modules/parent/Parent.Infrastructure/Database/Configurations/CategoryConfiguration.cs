using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Parent.Domain.Inventory.Category;

namespace System.Infrastructure.Database.Configurations;

public sealed class CategoryConfiguration : IEntityTypeConfiguration<CategoryM>
{
    public void Configure(EntityTypeBuilder<CategoryM> builder)
    {
        builder
            .HasKey(_ => _.CategoryId);

        builder
            .Property(_ => _.CategoryCode).IsRequired().HasMaxLength(50);

        builder
            .Property(_ => _.CategoryDesc).IsRequired().HasMaxLength(30);

        builder
            .HasIndex(_ => new { _.CategoryCode }).IsUnique();
    }
}

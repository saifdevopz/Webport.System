using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Tenant.Domain.Entities.INItem;

namespace Tenant.Infrastructure.Database.Configurations;

public sealed class ItemConfig : IEntityTypeConfiguration<INItemM>
{
    public void Configure(EntityTypeBuilder<INItemM> builder)
    {
        builder.HasKey(_ => _.ItemId);

        builder.HasIndex(_ => new { _.ItemCode }).IsUnique();

        builder.Property(_ => _.ItemCode)
               .IsRequired()
               .HasMaxLength(20);

        builder.Property(_ => _.ItemDesc)
               .IsRequired()
               .HasMaxLength(50);

        builder.HasOne(i => i.Category)
               .WithMany()
               .HasForeignKey(i => i.CategoryId)
               .OnDelete(DeleteBehavior.Restrict);
    }
}
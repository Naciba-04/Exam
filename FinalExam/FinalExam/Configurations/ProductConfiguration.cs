using FinalExam.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FinalExam.Configurations;

public class ProductConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.Property(x => x.FullName)
            .HasMaxLength(64)
            .IsRequired();
        builder.Property(x => x.ProfileImageUrl)
            .HasMaxLength(255)
            .IsRequired();
        builder.HasOne(x => x.Department)
            .WithMany(x => x.Products)
            .HasForeignKey(x => x.DepartmentId);
    }
}

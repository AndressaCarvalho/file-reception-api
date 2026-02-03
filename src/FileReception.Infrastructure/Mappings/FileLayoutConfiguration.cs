using FileReception.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace FileReception.Infrastructure.Mappings
{
    internal class FileLayoutConfiguration : IEntityTypeConfiguration<FileLayout>
    {
        public void Configure(EntityTypeBuilder<FileLayout> builder)
        {
            builder.ToTable("FileLayout");

            builder.HasKey(p => p.Id);

            builder.Property(p => p.Id).ValueGeneratedOnAdd();

            builder.Property(p => p.Name)
                .IsRequired()
                .HasMaxLength(255);

            builder.HasOne(p => p.Company)
                .WithMany(c => c.FileLayouts)
                .HasForeignKey(p => p.CompanyId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}

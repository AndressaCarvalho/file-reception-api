using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using FileReception.Domain.Entities;

namespace FileReception.Infrastructure.Mappings
{
    internal class FileLayoutFieldTypeConfiguration : IEntityTypeConfiguration<FileLayoutFieldType>
    {
        public void Configure(EntityTypeBuilder<FileLayoutFieldType> builder)
        {
            builder.ToTable("FileLayoutFieldType");

            builder.HasKey(p => p.Id);

            builder.Property(p => p.Id).ValueGeneratedOnAdd();

            builder.Property(p => p.Name)
                .IsRequired()
                .HasMaxLength(255);

            builder.HasIndex(p => p.Name)
                .IsUnique();

            builder.HasData(
                new FileLayoutFieldType { Id = 1, Name = "ALFA" },
                new FileLayoutFieldType { Id = 2, Name = "NUM" }
            );
        }
    }
}

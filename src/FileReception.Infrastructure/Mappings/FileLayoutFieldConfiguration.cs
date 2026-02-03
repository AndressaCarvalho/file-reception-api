using FileReception.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace FileReception.Infrastructure.Mappings
{
    internal class FileLayoutFieldConfiguration : IEntityTypeConfiguration<FileLayoutField>
    {
        public void Configure(EntityTypeBuilder<FileLayoutField> builder)
        {
            builder.ToTable("FileLayoutField");

            builder.HasKey(p => p.Id);

            builder.Property(p => p.Id).ValueGeneratedOnAdd();

            builder.HasOne(p => p.FileLayout)
                .WithMany(fl => fl.Fields)
                .HasForeignKey(p => p.FileLayoutId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Property(p => p.Description)
                .IsRequired()
                .HasMaxLength(255);

            builder.Property(p => p.Start)
                .IsRequired();

            builder.Property(p => p.End)
                .IsRequired();

            builder.HasOne(p => p.FileLayoutFieldType)
                .WithMany(flt => flt.Fields)
                .HasForeignKey(p => p.FileLayoutFieldTypeId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}

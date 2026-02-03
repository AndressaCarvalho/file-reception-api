using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using FileReception.Domain.Entities;

namespace FileReception.Infrastructure.Mappings
{
    internal class FileProcessConfiguration : IEntityTypeConfiguration<FileProcess>
    {
        public void Configure(EntityTypeBuilder<FileProcess> builder)
        {
            builder.ToTable("FileProcess");

            builder.HasKey(p => p.Id);

            builder.Property(p => p.Id).ValueGeneratedOnAdd();

            builder.HasOne(p => p.File)
                .WithMany(f => f.Processes)
                .HasForeignKey(p => p.FileId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Property(p => p.FilePathBackup)
                .IsRequired()
                .HasMaxLength(500);

            builder.Property(p => p.ReceivedAt)
                .IsRequired();

            builder.Property(p => p.IsValid)
                .IsRequired()
                .HasDefaultValue(false);

            builder.Property(p => p.ErrorMessage)
                .IsRequired(false)
                .HasMaxLength(1000);
        }
    }
}

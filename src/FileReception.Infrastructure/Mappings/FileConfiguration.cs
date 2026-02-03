using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using File = FileReception.Domain.Entities.File;

namespace FileReception.Infrastructure.Mappings
{
    internal class FileConfiguration : IEntityTypeConfiguration<File>
    {
        public void Configure(EntityTypeBuilder<File> builder)
        {
            builder.ToTable("File");

            builder.HasKey(p => p.Id);

            builder.Property(p => p.Id).ValueGeneratedOnAdd();

            builder.HasOne(p => p.FileLayout)
                .WithMany(fl => fl.Files)
                .HasForeignKey(p => p.FileLayoutId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Property(p => p.Name)
                .IsRequired()
                .HasMaxLength(255);

            builder.Property(p => p.ExpectedDate)
                .IsRequired();

            builder.HasOne(p => p.FileStatus)
                .WithMany(fs => fs.Files)
                .HasForeignKey(p => p.StatusId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Property(p => p.StatusId)
                .HasDefaultValue(1);
        }
    }
}

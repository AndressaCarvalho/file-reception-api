using FileReception.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace FileReception.Infrastructure.Mappings
{
    internal class FileStatusConfiguration : IEntityTypeConfiguration<FileStatus>
    {
        public void Configure(EntityTypeBuilder<FileStatus> builder)
        {
            builder.ToTable("FileStatus");

            builder.HasKey(p => p.Id);

            builder.Property(p => p.Id).ValueGeneratedOnAdd();

            builder.Property(p => p.Name)
                .IsRequired()
                .HasMaxLength(255);

            builder.HasIndex(p => p.Name)
                .IsUnique();

            builder.HasData(
                new FileStatus { Id = 1, Name = "Não Recepcionado" },
                new FileStatus { Id = 2, Name = "Recepcionado" },
                new FileStatus { Id = 3, Name = "Erro" }
            );
        }
    }
}

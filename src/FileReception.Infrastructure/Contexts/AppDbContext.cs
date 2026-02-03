using FileReception.Domain.Entities;
using FileReception.Infrastructure.Mappings;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace FileReception.Infrastructure.Contexts;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<Company> Company { get; set; }
    public DbSet<FileLayoutFieldType> FileLayoutFieldType { get; set; }
    public DbSet<FileLayoutField> FileLayoutField { get; set; }
    public DbSet<FileLayout> FileLayout { get; set; }
    public DbSet<FileStatus> FileStatus { get; set; }
    public DbSet<Domain.Entities.File> File { get; set; }
    public DbSet<FileProcess> FileProcess { get; set; }

    public class AppDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
    {
        public AppDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();

            var configuration = new ConfigurationBuilder()
                .SetBasePath(AppContext.BaseDirectory)
                .AddJsonFile("Configurations/appsettings.json")
                .Build();

            var connectionString = configuration.GetConnectionString("DefaultConnection");

            optionsBuilder.UseSqlServer(connectionString);

            return new AppDbContext(optionsBuilder.Options);
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfiguration(new CompanyConfiguration());
        modelBuilder.ApplyConfiguration(new FileLayoutFieldTypeConfiguration());
        modelBuilder.ApplyConfiguration(new FileLayoutFieldConfiguration());
        modelBuilder.ApplyConfiguration(new FileLayoutConfiguration());
        modelBuilder.ApplyConfiguration(new FileStatusConfiguration());
        modelBuilder.ApplyConfiguration(new FileConfiguration());
        modelBuilder.ApplyConfiguration(new FileProcessConfiguration());
    }
}

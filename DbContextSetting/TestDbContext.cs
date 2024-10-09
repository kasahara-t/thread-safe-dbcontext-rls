using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ThreadSafe;

namespace DbContextSetting;

public class TestDbContext : ThreadSafeDbContext
{
    public TestDbContext() : base(new DbContextOptionsBuilder().Options) {}

    public DbSet<SampleEntity> SampleEntities { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);
        optionsBuilder
            .UseSqlServer(@"Server=mssql; User Id=sa; Password=aYAJX1WhrK2B%6OUXN8MKR_oj; TrustServerCertificate=True")
            .EnableSensitiveDataLogging();
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<SampleEntity>()
            .HasQueryFilter(e => e.RLS == true);
    }
}

public class SampleEntity
{
    [Key]
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public bool RLS { get; set; } = false;
}
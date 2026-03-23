using DumpBillingProfileDataToDb.Entities;
using Microsoft.EntityFrameworkCore;

namespace DumpBillingProfileDataToDb.DBContext;

public class PostGresqlDBContext : DbContext
{
    // Constructor used by Dependency Injection
    public PostGresqlDBContext(DbContextOptions<PostGresqlDBContext> options)
        : base(options)
    {
    }

    // This DbSet is REQUIRED because your repository uses it
    public DbSet<NamePlate> NamePlateWithRegister { get; set; } = null!;

    //// Optional: Keep if you need it
    //public DbSet<BillingProfile> BillingProfiles { get; set; } = null!;

    // Fluent API configuration
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Since you're calling a stored procedure → no primary key
        modelBuilder.Entity<NamePlate>().HasNoKey();
    }
}
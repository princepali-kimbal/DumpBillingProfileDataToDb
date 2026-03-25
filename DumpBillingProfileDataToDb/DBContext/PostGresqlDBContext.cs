using DumpBillingProfileDataToDb.Entities;
using Microsoft.EntityFrameworkCore;

namespace DumpBillingProfileDataToDb.DBContext;

public class PostGresqlDBContext : DbContext
{
    // 🔹 Constructor (required for DI)
    public PostGresqlDBContext(DbContextOptions<PostGresqlDBContext> options)
        : base(options)
    {
    }

    // 🔹 NamePlate (from meter_nameplate or SP/view)
    public DbSet<NamePlate> NamePlateWithRegister { get; set; } = null!;

    // 🔹 Entity mapping table
    public DbSet<MeterEntity> MeterEntity { get; set; } = null!;

    // 🔹 Billing profile table
    public DbSet<MeterBillingProfile> MeterBillingProfile { get; set; } = null!;

    // 🔹 SubTemplate table (for DataFormat)
    public DbSet<MeterSubTemplate> MeterSubTemplate { get; set; } = null!;

    public DbSet<BillingProfileDto> BillingProfileDto { get; set; }
    public DbSet<MeterProfile> MeterProfiles { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // 🔴 NamePlate → No Key + Table Mapping
        modelBuilder.Entity<NamePlate>(entity =>
        {
            entity.HasNoKey();
            entity.ToTable("meter_nameplate");

            entity.Property(e => e.MeterNumber).HasColumnName("meternumber");
            entity.Property(e => e.MeterCategory).HasColumnName("metercategory");
        });

        modelBuilder.Entity<MeterProfile>(entity =>
        {
            entity.ToTable("meter_profiles");

            entity.HasKey(x => x.ProfileId);

            entity.Property(x => x.ProfileId).HasColumnName("profileid");
            entity.Property(x => x.ProfileName).HasColumnName("profilename");
        });

        // 🔴 MeterEntity
        modelBuilder.Entity<MeterEntity>(entity =>
        {
            entity.ToTable("meter_entity");

            entity.HasKey(e => new { e.MeterCategory, e.ProfileName, e.ObisCode });

            entity.Property(e => e.MeterCategory).HasColumnName("metercategory");
            entity.Property(e => e.ProfileName).HasColumnName("profilename");
            entity.Property(e => e.ObisCode).HasColumnName("obiscode");
            entity.Property(e => e.AttributeId).HasColumnName("attributeid");
            entity.Property(e => e.EntityCode).HasColumnName("entitycode");
            entity.Property(e => e.ObisId).HasColumnName("obisid");
        });

        // 🔴 MeterSubTemplate
        modelBuilder.Entity<MeterSubTemplate>(entity =>
        {
            entity.ToTable("meter_subtemplate");

            entity.HasKey(e => new
            {
                e.TemplateId,
                e.SubjectTypeName,
                e.MeterCategory,
                e.ProfileName,
                e.ObisCode
            });

            entity.Property(e => e.TemplateId).HasColumnName("templateid");
            entity.Property(e => e.SubjectTypeName).HasColumnName("subjecttypename");
            entity.Property(e => e.MeterCategory).HasColumnName("metercategory");
            entity.Property(e => e.ProfileName).HasColumnName("profilename");
            entity.Property(e => e.ObisCode).HasColumnName("obiscode");
            entity.Property(e => e.DataFormat).HasColumnName("dataformat");
        });

        modelBuilder.Entity<BillingProfileDto>().HasNoKey();

        // 🔴 MeterBillingProfile
        modelBuilder.Entity<MeterBillingProfile>(entity =>
        {
            entity.ToTable("meter_billingprofile");

            entity.HasKey(e => new
            {
                e.Projectid,
                e.Meternumber,
                e.Typeofmeter,
                e.Sourcetype,
                e.Isvalid,
                e.Rtcdateat,
                e.Mdmrequestid
            });

            entity.Property(e => e.Projectid).HasColumnName("projectid");
            entity.Property(e => e.Meternumber).HasColumnName("meternumber");
            entity.Property(e => e.Typeofmeter).HasColumnName("typeofmeter");
            entity.Property(e => e.Sourcetype).HasColumnName("sourcetype");
            entity.Property(e => e.Isvalid).HasColumnName("isvalid");
            entity.Property(e => e.Mdmrequestid).HasColumnName("mdmrequestid");
            entity.Property(e => e.CreatedAt).HasColumnName("createdat");
            entity.Property(e => e.Obisdata).HasColumnName("obisdata");
            entity.Property(e => e.ProfileName).HasColumnName("profilename");
            entity.Property(e => e.ProfileObisCode).HasColumnName("profileobiscode");
            entity.Property(e => e.Rtcdateat).HasColumnName("rtcdateat");
        });
    }
}
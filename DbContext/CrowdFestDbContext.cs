using CrowdFest.API.Configuration;
using CrowdFest.API.Entities;
using Microsoft.EntityFrameworkCore;

public class CrowdFestDbContext: DbContext
{
    private readonly IEncryptionService _encryption;
    public CrowdFestDbContext(DbContextOptions<CrowdFestDbContext> options, IEncryptionService encryption) : base(options) 
    { 
        _encryption = encryption;
    }

    public DbSet<EventEntity> events { get; set; }
    public DbSet<GroupEntity> groups { get; set; }
    public DbSet<LocationEntity> locations { get; set; }
    public DbSet<PlannerEntity> planners { get; set; }
    public DbSet<PlannerGroupEntity> plannerGroups { get; set; }
    public DbSet<PlannerAccountEntity> plannerAccounts { get; set; }
    public DbSet<PostEntity> posts { get; set; }
    public DbSet<ThemeEntity> themes { get; set; }
    public DbSet<VoteEntity> votes { get; set; }
    public DbSet<OrganizationEntity> organisations { get; set; }
    public DbSet<OrganizationAccountEntity> organisationAccounts { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        //Configuartions
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfiguration(new EventConfiguration());
        modelBuilder.ApplyConfiguration(new GroupConfiguration());
        modelBuilder.ApplyConfiguration(new LocationConfiguration());
        modelBuilder.ApplyConfiguration(new PlannerConfiguration());
        modelBuilder.ApplyConfiguration(new PlannerGroupConfiguration());
        modelBuilder.ApplyConfiguration(new OrganizationConfiguration());
        modelBuilder.ApplyConfiguration(new ThemeConfiguration());
        modelBuilder.ApplyConfiguration(new VoteConfiguration());

        //Encryption for data
        modelBuilder.Entity<PlannerEntity>()
            .Property(p => p.displayName)
            .HasConversion(
                p => _encryption.Encrypt(p),
                p => _encryption.Decrypt(p)
            );

        modelBuilder.Entity<PlannerEntity>()
            .Property(p => p.firstName)
            .HasConversion(
                p => _encryption.Encrypt(p),
                p => _encryption.Decrypt(p)
            );
        
        modelBuilder.Entity<PlannerEntity>()
            .Property(p => p.lastName)
            .HasConversion(
                p => _encryption.Encrypt(p),
                p => _encryption.Decrypt(p)
            );
        
        modelBuilder.Entity<PlannerEntity>()
            .Property(p => p.emailAddress)
            .HasConversion(
                p => _encryption.Encrypt(p),
                p => _encryption.Decrypt(p)
            );
        
        modelBuilder.Entity<PlannerEntity>()
            .Property(p => p.phoneNumber)
            .HasConversion(
                p => _encryption.Encrypt(p),
                p => _encryption.Decrypt(p)
            );
    }
}
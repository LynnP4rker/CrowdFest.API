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
    public DbSet<PostEntity> posts { get; set; }
    public DbSet<ThemeEntity> themes { get; set; }
    public DbSet<UserEntity> users { get; set; }
    public DbSet<VoteEntity> votes { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        //Configuartions
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfiguration(new EventConfiguration());
        modelBuilder.ApplyConfiguration(new GroupConfiguration());
        modelBuilder.ApplyConfiguration(new LocationConfiguration());
        modelBuilder.ApplyConfiguration(new PlannerConfiguration());
        modelBuilder.ApplyConfiguration(new PlannerGroupConfiguration());
        modelBuilder.ApplyConfiguration(new ThemeConfiguration());
        modelBuilder.ApplyConfiguration(new VoteConfiguration());

        //Encryption for data
        modelBuilder.Entity<UserEntity>()
            .Property(u => u.emailAddress)
            .HasConversion(
                u => _encryption.Encrypt(u),
                u => _encryption.Decrypt(u)
            );

        modelBuilder.Entity<UserEntity>()
            .Property(u => u.firstName)
            .HasConversion(
                u => _encryption.Encrypt(u),
                u => _encryption.Decrypt(u)
            );

        modelBuilder.Entity<UserEntity>()
            .Property(u => u.lastName)
            .HasConversion(
                u => _encryption.Encrypt(u),
                u => _encryption.Decrypt(u)
            );
        
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
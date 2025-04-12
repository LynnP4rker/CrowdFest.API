using CrowdFest.API.Entities;
using Microsoft.EntityFrameworkCore;

public class CrowdFestDbContext: DbContext
{
    public CrowdFestDbContext(DbContextOptions<CrowdFestDbContext> options) : base(options) { }

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
        // For future Lynn, encryption for PII in the fields go here
    }
}
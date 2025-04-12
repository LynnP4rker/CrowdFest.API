using CrowdFest.API.Common;
using CrowdFest.API.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CrowdFest.API.Configuration;

public sealed class PlannerConfiguration : IEntityTypeConfiguration<PlannerEntity>
{
    void IEntityTypeConfiguration<PlannerEntity>.Configure(EntityTypeBuilder<PlannerEntity> builder) 
    {
        //Priority configuration
        builder
            .Property(p => p.gender)
            .HasConversion<string>();

        //Relationship configuration
        builder
            .HasMany<PlannerGroupEntity>()
            .WithOne()
            .HasForeignKey(p => p.plannerId)
            .OnDelete(DeleteBehavior.Cascade);
        
        builder
            .HasOne<PostEntity>()
            .WithOne()
            .HasForeignKey<PostEntity>(p => p.plannerId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
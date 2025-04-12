using CrowdFest.API.Common;
using CrowdFest.API.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CrowdFest.API.Configuration;

public sealed class LocationConfiguration : IEntityTypeConfiguration<LocationEntity>
{
    void IEntityTypeConfiguration<LocationEntity>.Configure(EntityTypeBuilder<LocationEntity> builder)
    {
        //Property Configuration
        builder
            .HasIndex(l => l.nameNormalised)
            .IsUnique();
        
        //Relationship configuration
        builder
            .HasKey(l => l.locationId);
            
        builder
            .HasOne<PlannerEntity>()
            .WithMany()
            .HasForeignKey(t => t.plannerId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
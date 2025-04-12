using CrowdFest.API.Common;
using CrowdFest.API.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CrowdFest.API.Configuration;

public sealed class PlannerConfiguration : IEntityTypeConfiguration<PlannerEntity>
{
    void IEntityTypeConfiguration<PlannerEntity>.Configure(EntityTypeBuilder<PlannerEntity> builder) 
    {
        //Property configuration
        builder
            .Property(p => p.displayName)
            .HasMaxLength(Constants.DISPLAYNAMELENGTH);
        
        builder
            .Property(p => p.firstName)
            .HasMaxLength(Constants.FIRSTNAMELENGTH);
        
        builder
            .Property(p => p.lastName)
            .HasMaxLength(Constants.LASTNAMELENGTH);
        
        builder
            .Property(p => p.emailAddress)
            .HasMaxLength(Constants.EMAILADDRESSLENGTH);
        
        builder
            .Property(p => p.phoneNumber)
            .HasMaxLength(Constants.PHONENUMBERLENGTH);

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
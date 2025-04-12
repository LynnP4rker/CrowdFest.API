using CrowdFest.API.Common;
using CrowdFest.API.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CrowdFest.API.Configuration;

public sealed class VoteConfiguration : IEntityTypeConfiguration<VoteEntity>
{
    void IEntityTypeConfiguration<VoteEntity>.Configure(EntityTypeBuilder<VoteEntity> builder) 
    {
        builder
            .Property(v => v.priority)
            .HasConversion<string>();

        builder
            .HasOne<LocationEntity>()
            .WithOne()
            .HasForeignKey<VoteEntity>(l => l.locationId)
            .OnDelete(DeleteBehavior.NoAction);
        
        builder
            .HasOne<ThemeEntity>()
            .WithOne()
            .HasForeignKey<VoteEntity>(t => t.themeId)
            .OnDelete(DeleteBehavior.NoAction);
    }
}
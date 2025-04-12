using CrowdFest.API.Common;
using CrowdFest.API.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CrowdFest.API.Configuration;

public sealed class EventConfiguration : IEntityTypeConfiguration<EventEntity>
{
    void IEntityTypeConfiguration<EventEntity>.Configure(EntityTypeBuilder<EventEntity> builder)
    {
        //Property Configuration
        builder
            .Property(e => e.priority)
            .HasConversion<string>();
        
        builder
            .HasOne<LocationEntity>()
            .WithOne()
            .HasForeignKey<EventEntity>(l => l.locationId)
            .OnDelete(DeleteBehavior.NoAction);
        
        builder
            .HasOne<ThemeEntity>()
            .WithOne()
            .HasForeignKey<EventEntity>(t => t.themeId)
            .OnDelete(DeleteBehavior.NoAction);
        
        //Relationship configuration
        builder
            .HasOne<GroupEntity>()
            .WithMany()
            .HasForeignKey(e => e.groupId)
            .OnDelete(DeleteBehavior.Cascade);
        
        builder
            .HasOne<VoteEntity>()
            .WithOne()
            .HasForeignKey<VoteEntity>(v => v.eventId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}

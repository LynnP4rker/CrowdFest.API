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
            .WithMany()
            .HasForeignKey(l => l.locationId);

        builder
            .HasOne<ThemeEntity>()
            .WithMany()
            .HasForeignKey(t => t.themeId);
        
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

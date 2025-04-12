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
            .Property(e => e.title)
            .HasMaxLength(Constants.TITLELENGTH);
        
        builder
            .Property(e => e.description)
            .HasMaxLength(Constants.DESCRIPTIONLENGTH);
        
        builder
            .Property(e => e.location)
            .HasMaxLength(Constants.LOCATIONLENGTH);
        
        builder
            .Property(e => e.theme)
            .HasMaxLength(Constants.THEMELENGTH);
        
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

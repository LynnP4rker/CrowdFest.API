using CrowdFest.API.Common;
using CrowdFest.API.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CrowdFest.API.Configuration;

public sealed class GroupConfiguration : IEntityTypeConfiguration<GroupEntity>
{
    void IEntityTypeConfiguration<GroupEntity>.Configure(EntityTypeBuilder<GroupEntity> builder)
    {
        //Property configuration
        builder
            .HasIndex(g => g.NameNormalised)
            .IsUnique();
        
        //Relationship configuration
        builder
            .HasOne<VoteEntity>()
            .WithOne()
            .HasForeignKey<VoteEntity>(v => v.groupId)
            .OnDelete(DeleteBehavior.Cascade);
        
        builder
            .HasMany<PlannerGroupEntity>()
            .WithOne()
            .HasForeignKey(p => p.groupId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
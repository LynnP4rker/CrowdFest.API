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
            .Property(g => g.name)
            .HasMaxLength(Constants.GROUPLENGTH);
        
        builder
            .HasIndex(g => g.nameNormalised)
            .IsUnique();
        
        builder
            .Property(g => g.description)
            .HasMaxLength(Constants.DESCRIPTIONLENGTH);
        
        //Relationship configuration
        builder
            .HasOne<VoteEntity>()
            .WithOne()
            .HasForeignKey<VoteEntity>(v => v.groupId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
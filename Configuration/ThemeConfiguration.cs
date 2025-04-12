using CrowdFest.API.Common;
using CrowdFest.API.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CrowdFest.API.Configuration;

public sealed class ThemeConfiguration : IEntityTypeConfiguration<ThemeEntity>
{
    void IEntityTypeConfiguration<ThemeEntity>.Configure(EntityTypeBuilder<ThemeEntity> builder)
    {
        //Property Configuration
        builder
            .Property(t => t.name)
            .HasMaxLength(Constants.THEMELENGTH);
        
        builder
            .HasIndex(t => t.nameNormalised)
            .IsUnique();
        
        builder
            .Property(t => t.description)
            .HasMaxLength(Constants.DESCRIPTIONLENGTH);
        
        //Relationship Configuration
        builder
            .HasKey(t => t.themeId);
        
        builder
            .HasOne<PlannerEntity>()
            .WithMany()
            .HasForeignKey(t => t.plannerId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
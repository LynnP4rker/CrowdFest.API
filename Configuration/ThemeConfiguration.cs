using CrowdFest.API.Common;
using CrowdFest.API.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CrowdFest.API.Configuration;

public sealed class ThemeConfiguration : IEntityTypeConfiguration<ThemeEntity>
{
    void IEntityTypeConfiguration<ThemeEntity>.Configure(EntityTypeBuilder<ThemeEntity> builder)
    {
        builder
            .Property(t => t.name)
            .HasMaxLength(Constants.THEMELENGTH);
        
        builder
            .HasIndex(t => t.nameNormalised)
            .IsUnique();
        
        builder
            .Property(t => t.description)
            .HasMaxLength(Constants.DESCRIPTIONLENGTH);
    }
}
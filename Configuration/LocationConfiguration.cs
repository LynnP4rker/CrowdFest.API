using CrowdFest.API.Common;
using CrowdFest.API.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CrowdFest.API.Configuration;

public sealed class LocationConfiguration : IEntityTypeConfiguration<LocationEntity>
{
    void IEntityTypeConfiguration<LocationEntity>.Configure(EntityTypeBuilder<LocationEntity> builder)
    {
        builder
            .Property(l => l.name)
            .HasMaxLength(Constants.LOCATIONLENGTH);
        
        builder
            .HasIndex(l => l.nameNormalised)
            .IsUnique();
        
        builder
            .Property(l => l.description)
            .HasMaxLength(Constants.DESCRIPTIONLENGTH);
    }
}
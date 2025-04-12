using CrowdFest.API.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CrowdFest.API.Configuration;

public sealed class PlannerGroupConfiguration : IEntityTypeConfiguration<PlannerGroupEntity>
{
    void IEntityTypeConfiguration<PlannerGroupEntity>.Configure(EntityTypeBuilder<PlannerGroupEntity> builder) =>
        builder
            .HasKey(p => new {p.plannerId, p.groupId});
}
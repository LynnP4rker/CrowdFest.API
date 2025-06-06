using CrowdFest.API.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CrowdFest.API.Configuration;

public sealed class OrganizationConfiguration: IEntityTypeConfiguration<OrganizationEntity>
{
    void IEntityTypeConfiguration<OrganizationEntity>.Configure(EntityTypeBuilder<OrganizationEntity> builder)
    {
        //Relationship configuration
        builder 
            .HasOne<OrganizationAccountEntity>()
            .WithOne()
            .HasForeignKey<OrganizationAccountEntity>(o => o.id)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
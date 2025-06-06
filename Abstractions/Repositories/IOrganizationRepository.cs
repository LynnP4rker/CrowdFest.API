using CrowdFest.API.Entities;

namespace CrowdFest.API.Abstractions.Repositories;

public interface IOrganizationRepository
{
    Task CreateAsync(OrganizationEntity organizationEntity, CancellationToken cancellationToken);
    Task DeleteAsync(OrganizationEntity organizationEntity, CancellationToken cancellationToken);
    Task <IEnumerable<OrganizationEntity>> ListAsync(CancellationToken cancellationToken);
    Task <OrganizationEntity?> RetrieveAsync(Guid id, CancellationToken cancellationToken);
    Task<OrganizationEntity?> RetrieveEmailAsync(string emailAddress, CancellationToken cancellationToken);
    Task UpdateAsync(OrganizationEntity organizationEntity, CancellationToken cancellationToken);
    Task SaveChangesAsync(CancellationToken cancellationToken);
}
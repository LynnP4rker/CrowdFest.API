using CrowdFest.API.Entities;

namespace CrowdFest.API.Abstractions.Repositories;
public interface IOrganizationAccountRepository
{
    Task CreateAsync(OrganizationAccountEntity organisationAccountEntity, CancellationToken cancellationToken);
    Task DeleteAsync(OrganizationAccountEntity organisationAccountEntity, CancellationToken cancellationToken);
    Task <OrganizationAccountEntity?> RetrieveAsync(Guid id, CancellationToken cancellationToken);
    Task UpdateAsync(OrganizationAccountEntity organisationAccountEntity, CancellationToken cancellationToken);
    Task SaveChangesAsync(CancellationToken cancellationToken);
}
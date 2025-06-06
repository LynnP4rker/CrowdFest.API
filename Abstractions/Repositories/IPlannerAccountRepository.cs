using CrowdFest.API.Entities;

namespace CrowdFest.API.Abstractions.Repositories;
public interface IPlannerAccountRepository
{
    Task CreateAsync(PlannerAccountEntity plannerAccountEntity, CancellationToken cancellationToken);
    Task DeleteAsync(PlannerAccountEntity plannerAccountEntity, CancellationToken cancellationToken);
    Task <PlannerAccountEntity?> RetrieveAsync(Guid id, CancellationToken cancellationToken);
    Task UpdateAsync(PlannerAccountEntity plannerAccountEntity, CancellationToken cancellationToken);
    Task SaveChangesAsync(CancellationToken cancellationToken);
}
using CrowdFest.API.Entities;

namespace CrowdFest.API.Abstractions.Repositories;

public interface IPlannerRepository 
{
    Task CreateAsync(PlannerEntity plannerEntity, CancellationToken cancellationToken);
    Task DeleteAsync(PlannerEntity plannerEntity, CancellationToken cancellationToken);
    Task<IEnumerable<PlannerEntity>> ListAsync(CancellationToken cancellationToken);
    Task<PlannerEntity?> RetrieveAsync(Guid id, CancellationToken cancellationToken);
    Task<PlannerEntity?> RetrieveEmailAsync(string emailAddress, CancellationToken cancellationToken);
    Task UpdateAsync(PlannerEntity plannerEntity, CancellationToken cancellationToken);
    Task SaveChangesAsync(CancellationToken cancellationToken);

}
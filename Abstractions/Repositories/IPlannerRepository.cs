using CrowdFest.API.Entities;

namespace CrowdFest.API.Abstractions.Repositories;

public interface IPlannerRepository 
{
    Task CreateAsync(PlannerEntity plannerEntity, CancellationToken cancellationToken);
    Task DeleteAsync(PlannerEntity plannerEntity, CancellationToken cancellationToken);
    Task<IEnumerable<PlannerEntity>> ListAsync(CancellationToken cancellationToken);
    Task<PlannerEntity> RetrieveAsync(int id, CancellationToken cancellationToken);
    Task UpdateAsync(PlannerEntity plannerEntity, CancellationToken cancellationToken);

}
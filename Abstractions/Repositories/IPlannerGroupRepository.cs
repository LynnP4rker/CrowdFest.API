using CrowdFest.API.Entities;

namespace CrowdFest.API.Abstractions.Repositories;

public interface IPlannerGroupRepository
{
    Task CreateAsync(PlannerGroupEntity plannerGroupEntity, CancellationToken cancellationToken);
    Task DeleteAsync(PlannerGroupEntity plannerGroupEntity, CancellationToken cancellationToken);
    Task<IEnumerable<PlannerGroupEntity>> ListAsync(CancellationToken cancellationToken);
    Task<PlannerGroupEntity?> RetrieveAsync(Guid plannerId, Guid groupId, CancellationToken cancellationToken);
    Task UpdateAsync(PlannerGroupEntity plannerGroupEntity, CancellationToken cancellationToken);
}

using CrowdFest.API.Abstractions.Repositories;
using CrowdFest.API.Entities;

namespace CrowdFest.API.Repositories;

internal sealed class PlannerGroupRepository : IPlannerGroupRepository
{
    private readonly CrowdFestDbContext _context;
    
    public PlannerGroupRepository(CrowdFestDbContext context)
    {
        _context = context;
    }
    public Task CreateAsync(PlannerGroupEntity plannerGroupEntity, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task DeleteAsync(PlannerGroupEntity plannerGroupEntity, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<PlannerGroupEntity>> ListAsync(CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<PlannerGroupEntity> RetrieveAsync(int id, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task UpdateAsync(PlannerGroupEntity plannerGroupEntity, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
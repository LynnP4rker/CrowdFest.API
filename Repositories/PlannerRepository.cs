using CrowdFest.API.Abstractions.Repositories;
using CrowdFest.API.Entities;

namespace CrowdFest.API.Repositories;

internal sealed class PlannerRepository : IPlannerRepository
{
    private readonly CrowdFestDbContext _context;
    public PlannerRepository(CrowdFestDbContext context)
    {
        _context = context;
    }
    public Task CreateAsync(PlannerEntity plannerEntity, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task DeleteAsync(PlannerEntity plannerEntity, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<PlannerEntity>> ListAsync(CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<PlannerEntity> RetrieveAsync(int id, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task UpdateAsync(PlannerEntity plannerEntity, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
using CrowdFest.API.Abstractions.Repositories;
using CrowdFest.API.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

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
        return _context.planners
                .AddAsync(plannerEntity, cancellationToken)
                .AsTask();
    }

    public Task DeleteAsync(PlannerEntity plannerEntity, CancellationToken cancellationToken)
    {
        _context.planners.Remove(plannerEntity);
        return Task.CompletedTask;
    }

    public async Task<IEnumerable<PlannerEntity>> ListAsync(CancellationToken cancellationToken)
    {
        IEnumerable<PlannerEntity> entities = 
            await _context.planners
                .AsNoTracking()
                .ToArrayAsync(cancellationToken);
        
        return entities;
    }

    public Task<PlannerEntity?> RetrieveAsync(Guid id, CancellationToken cancellationToken)
    {
        return _context.planners
            .AsNoTracking()
            .FirstOrDefaultAsync(p => p.id.Equals(id), cancellationToken);
    }

    public Task UpdateAsync(PlannerEntity plannerEntity, CancellationToken cancellationToken)
    {
        EntityEntry<PlannerEntity> entry = _context.planners.Entry(plannerEntity);
        entry.State = EntityState.Modified;
        return Task.CompletedTask;
    }
}
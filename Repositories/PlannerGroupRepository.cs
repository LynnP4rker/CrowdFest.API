using CrowdFest.API.Abstractions.Repositories;
using CrowdFest.API.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

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
        return _context.plannerGroups
                .AddAsync(plannerGroupEntity, cancellationToken)
                .AsTask();
    }

    public Task DeleteAsync(PlannerGroupEntity plannerGroupEntity, CancellationToken cancellationToken)
    {
        _context.plannerGroups.Remove(plannerGroupEntity);
        return Task.CompletedTask;
    }

    public async Task<IEnumerable<PlannerGroupEntity>> ListAsync(CancellationToken cancellationToken)
    {
        IEnumerable<PlannerGroupEntity> entities = 
            await _context.plannerGroups
                .AsNoTracking()
                .ToArrayAsync(cancellationToken);
        
        return entities;
    }

    public Task<PlannerGroupEntity?> RetrieveAsync(Guid plannerId, Guid groupId,  CancellationToken cancellationToken)
    {
        return _context.plannerGroups
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.plannerId.Equals(plannerId) && p.groupId.Equals(groupId), cancellationToken);
    }

    public Task UpdateAsync(PlannerGroupEntity plannerGroupEntity, CancellationToken cancellationToken)
    {
        EntityEntry<PlannerGroupEntity> entry = _context.plannerGroups.Entry(plannerGroupEntity);
        entry.State = EntityState.Modified;
        return Task.CompletedTask;
    }
}
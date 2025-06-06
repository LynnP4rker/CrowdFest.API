using CrowdFest.API.Abstractions.Repositories;
using CrowdFest.API.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

internal sealed class PlannerAccountRepository : IPlannerAccountRepository
{
    private readonly CrowdFestDbContext _context;
    public PlannerAccountRepository(CrowdFestDbContext context)
    {
        _context = context;
    }
    public Task CreateAsync(PlannerAccountEntity plannerAccountEntity, CancellationToken cancellationToken)
    {
        return _context.plannerAccounts
            .AddAsync(plannerAccountEntity, cancellationToken)
            .AsTask();
    }

    public Task DeleteAsync(PlannerAccountEntity plannerAccountEntity, CancellationToken cancellationToken)
    {
        _context.plannerAccounts.Remove(plannerAccountEntity);
        return Task.CompletedTask;
    }

    public Task<PlannerAccountEntity?> RetrieveAsync(Guid id, CancellationToken cancellationToken)
    {
        return _context.plannerAccounts
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.id.Equals(id), cancellationToken);
    }

    public Task SaveChangesAsync(CancellationToken cancellationToken)
    {
        return _context.SaveChangesAsync(cancellationToken);
    }

    public Task UpdateAsync(PlannerAccountEntity plannerAccountEntity, CancellationToken cancellationToken)
    {
        EntityEntry<PlannerAccountEntity> entry = _context.plannerAccounts.Entry(plannerAccountEntity);
        entry.State = EntityState.Modified;
        return Task.CompletedTask;
    }
}
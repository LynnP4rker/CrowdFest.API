using CrowdFest.API.Abstractions.Repositories;
using CrowdFest.API.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace CrowdFest.API.Repositories;

internal sealed class GroupRepository : IGroupRepository
{
    private readonly CrowdFestDbContext _context;
    
    public GroupRepository(CrowdFestDbContext context)
    {
        _context = context;
    }
    public Task CreateAsync(GroupEntity groupEntity, CancellationToken cancellationToken)
    {
        return _context.groups
                .AddAsync(groupEntity, cancellationToken)
                .AsTask();
    }

    public Task DeleteAsync(GroupEntity groupEntity, CancellationToken cancellationToken)
    {
        _context.groups.Remove(groupEntity);
        return Task.CompletedTask;
    }

    public async Task<IEnumerable<GroupEntity>> ListAsync(CancellationToken cancellationToken)
    {
        IEnumerable<GroupEntity> entities = 
            await _context.groups
                .AsNoTracking()
                .ToArrayAsync(cancellationToken);
        
        return entities;
    }

    public Task<GroupEntity?> RetrieveAsync(Guid id, CancellationToken cancellationToken)
    {
        return 
            _context.groups
                .AsNoTracking()
                .FirstOrDefaultAsync(g => g.id.Equals(id), cancellationToken);
    }

    public Task SaveChangesAsync(CancellationToken cancellationToken)
    {
        return _context.SaveChangesAsync();
    }

    public Task UpdateAsync(GroupEntity groupEntity, CancellationToken cancellationToken)
    {
        EntityEntry<GroupEntity> entry = _context.groups.Entry(groupEntity);
        entry.State = EntityState.Modified;
        return Task.CompletedTask;
    }
}
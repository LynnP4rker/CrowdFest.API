using CrowdFest.API.Abstractions.Repositories;
using CrowdFest.API.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace CrowdFest.API.Repositories;

internal sealed class EventRepository : IEventRepository
{
    private readonly CrowdFestDbContext _context;

    public EventRepository(CrowdFestDbContext context)
    {
        _context = context;
    }

    public Task CreateAsync(EventEntity eventEntity, CancellationToken cancellationToken)
    {
        return _context.events.AddAsync(eventEntity, cancellationToken)
        .AsTask();
    }

    public Task DeleteAsync(EventEntity eventEntity, CancellationToken cancellationToken)
    {
        _context.events.Remove(eventEntity);
        return Task.CompletedTask;
    }

    public async Task<IEnumerable<EventEntity>> ListAsync(CancellationToken cancellationToken)
    {
        IEnumerable<EventEntity> eventEntities = 
            await _context.events
                .AsNoTracking()
                .ToArrayAsync(cancellationToken);
        
        return eventEntities;
    }

    public Task<EventEntity?> RetrieveAsync(Guid id, CancellationToken cancellationToken)
    {
        return _context.events
            .AsNoTracking()
            .FirstOrDefaultAsync(e => e.id.Equals(id), cancellationToken);
    }

    public Task UpdateAsync(EventEntity eventEntity, CancellationToken cancellationToken)
    {
        EntityEntry<EventEntity> entry = _context.events.Entry(eventEntity);
        entry.State = EntityState.Modified;
        return Task.CompletedTask;
    }
}
using CrowdFest.API.Abstractions.Repositories;
using CrowdFest.API.Entities;

namespace CrowdFest.API.Repositories;

internal sealed class EventRepository : IEventRepository
{
    private readonly CrowdFestDbContext _context;

    public EventRepository(CrowdFestDbContext context)
    {
        _context = context;
    }

    public async Task CreateAsync(EventEntity eventEntity, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public async Task DeleteAsync(EventEntity eventEntity, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public async Task<IEnumerable<EventEntity>> ListAsync(CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public async Task<EventEntity> RetrieveAsync(int id, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task UpdateAsync(EventEntity eventEntity, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
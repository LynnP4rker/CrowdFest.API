using CrowdFest.API.Abstractions.Repositories;
using CrowdFest.API.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace CrowdFest.API.Repositories;

internal sealed class LocationRepository : ILocationRepository
{
    private readonly CrowdFestDbContext _context;
    
    public LocationRepository(CrowdFestDbContext context)
    {
        _context = context;
    }

    public Task CreateAsync(LocationEntity locationEntity, CancellationToken cancellationToken)
    {
        return _context.locations
                .AddAsync(locationEntity, cancellationToken)
                .AsTask();
    }

    public Task DeleteAsync(LocationEntity locationEntity, CancellationToken cancellationToken)
    {
        _context.locations.Remove(locationEntity);
        return Task.CompletedTask;
    }

    public async Task<IEnumerable<LocationEntity>> ListAsync(CancellationToken cancellationToken)
    {
        IEnumerable<LocationEntity> entities = 
            await _context.locations
                .AsNoTracking()
                .ToArrayAsync(cancellationToken);
        
        return entities;
    }

    public Task<LocationEntity?> RetrieveAsync(Guid id, CancellationToken cancellationToken)
    {
        return _context.locations
            .AsNoTracking()
            .FirstOrDefaultAsync(l => l.locationId.Equals(id), cancellationToken);
    }

    public Task SaveChangesAsync(CancellationToken cancellationToken)
    {
        return _context.SaveChangesAsync();
    }

    public Task UpdateAsync(LocationEntity locationEntity, CancellationToken cancellationToken)
    {
        EntityEntry<LocationEntity> entry = _context.locations.Entry(locationEntity);
        entry.State = EntityState.Modified;
        return Task.CompletedTask;
    }
}
using CrowdFest.API.Abstractions.Repositories;
using CrowdFest.API.Entities;

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
        throw new NotImplementedException();
    }

    public Task DeleteAsync(LocationEntity locationEntity, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<LocationEntity>> ListAsync(CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<LocationEntity> RetrieveAsync(int id, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task UpdateAsync(LocationEntity locationEntity, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
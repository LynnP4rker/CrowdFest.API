using CrowdFest.API.Entities;

namespace CrowdFest.API.Abstractions.Repositories;

public interface ILocationRepository
{
    Task CreateAsync(LocationEntity locationEntity, CancellationToken cancellationToken);
    Task DeleteAsync(LocationEntity locationEntity, CancellationToken cancellationToken);
    Task<IEnumerable<LocationEntity>> ListAsync(CancellationToken cancellationToken);
    Task<LocationEntity> RetrieveAsync(int id, CancellationToken cancellationToken);
    Task UpdateAsync(LocationEntity locationEntity, CancellationToken cancellationToken);
}
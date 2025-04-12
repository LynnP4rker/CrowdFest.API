using CrowdFest.API.Entities;

namespace CrowdFest.API.Abstractions.Repositories;

public interface IGroupRepository
{
    Task CreateAsync(GroupEntity groupEntity, CancellationToken cancellationToken);
    Task DeleteAsync(GroupEntity groupEntity, CancellationToken cancellationToken);
    Task<IEnumerable<GroupEntity>> ListAsync(CancellationToken cancellationToken);
    Task<GroupEntity> RetrieveAsync(int id, CancellationToken cancellationToken);
    Task UpdateAsync(GroupEntity groupEntity, CancellationToken cancellationToken);
}
using CrowdFest.API.Abstractions.Repositories;
using CrowdFest.API.Entities;

namespace CrowdFest.API.Repositories;

internal sealed class GroupRepository : IGroupRepository
{
    public Task CreateAsync(GroupEntity groupEntity, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task DeleteAsync(GroupEntity groupEntity, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<GroupEntity>> ListAsync(CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<GroupEntity> RetrieveAsync(int id, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task UpdateAsync(GroupEntity groupEntity, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
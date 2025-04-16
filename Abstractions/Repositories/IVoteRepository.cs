using CrowdFest.API.Entities;

namespace CrowdFest.API.Abstractions.Repositories;

public interface IVoteRepository
{
    Task CreateAsync(VoteEntity voteEntity, CancellationToken cancellationToken);
    Task DeleteAsync(VoteEntity voteEntity, CancellationToken cancellationToken);
    Task<IEnumerable<VoteEntity>> ListAsync(Guid groupId, CancellationToken cancellationToken);
    Task<VoteEntity?> RetrieveAsync(Guid id, Guid groupId, Guid eventId, CancellationToken cancellationToken);
    Task UpdateAsync(VoteEntity voteEntity, CancellationToken cancellationToken);
    Task SaveChangesAsync(CancellationToken cancellationToken);
}
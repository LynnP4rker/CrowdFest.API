using CrowdFest.API.Entities;

namespace CrowdFest.API.Abstractions.Repositories;

public interface IVoteRepository
{
    Task CreateAsync(VoteEntity voteEntity, CancellationToken cancellationToken);
    Task DeleteAsync(VoteEntity voteEntity, CancellationToken cancellationToken);
    Task<IEnumerable<VoteEntity>> ListVotesForGroupAsync(Guid groupId, CancellationToken cancellationToken);
    Task<IEnumerable<VoteEntity>> ListVotesForPlannerAsync(Guid plannerId, CancellationToken cancellationToken);
    Task<VoteEntity?> RetrieveAsync(Guid id, Guid plannerId, CancellationToken cancellationToken);
    Task UpdateAsync(VoteEntity voteEntity, CancellationToken cancellationToken);
    Task SaveChangesAsync(CancellationToken cancellationToken);
}
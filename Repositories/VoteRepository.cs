using CrowdFest.API.Abstractions.Repositories;
using CrowdFest.API.Entities;

namespace CrowdFest.API.Repositories;

internal sealed class VoteRepository : IVoteRepository
{
    private readonly CrowdFestDbContext _context;
    public VoteRepository(CrowdFestDbContext context)
    {
        _context = context;
    }
    public Task CreateAsync(VoteEntity voteEntity, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task DeleteAsync(VoteEntity voteEntity, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<VoteEntity>> ListAsync(CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<VoteEntity> RetrieveAsync(int id, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task UpdateAsync(VoteEntity voteEntity, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
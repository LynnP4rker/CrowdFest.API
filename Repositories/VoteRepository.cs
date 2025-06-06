using CrowdFest.API.Abstractions.Repositories;
using CrowdFest.API.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

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
        return _context.votes
                .AddAsync(voteEntity, cancellationToken)
                .AsTask();
    }

    public Task DeleteAsync(VoteEntity voteEntity, CancellationToken cancellationToken)
    {
        _context.votes.Remove(voteEntity);
        return Task.CompletedTask;
    }

    public async Task<IEnumerable<VoteEntity>> ListVotesForGroupAsync(Guid groupId, CancellationToken cancellationToken)
    {
        IEnumerable<VoteEntity> entities = 
            await _context.votes
                .AsNoTracking()
                .Where(v => v.groupId.Equals(groupId))
                .ToListAsync();
        
        return entities;
    }

    public async Task<IEnumerable<VoteEntity>> ListVotesForPlannerAsync(Guid plannerId, CancellationToken cancellationToken)
    {
        IEnumerable<VoteEntity> entities = 
            await _context.votes
                .AsNoTracking()
                .Where(v => v.plannerId.Equals(plannerId))
                .ToListAsync();
        
        return entities;
    }

    public Task<VoteEntity?> RetrieveAsync(Guid id, Guid plannerId, CancellationToken cancellationToken)
    {
        return _context.votes
            .AsNoTracking()
            .Where(v => v.id.Equals(id) && v.plannerId.Equals(plannerId))
            .FirstOrDefaultAsync(v => v.id.Equals(id), cancellationToken);
    }

    public Task SaveChangesAsync(CancellationToken cancellationToken)
    {
        return _context.SaveChangesAsync();
    }

    public Task UpdateAsync(VoteEntity voteEntity, CancellationToken cancellationToken)
    {
        EntityEntry<VoteEntity> entry = _context.votes.Entry(voteEntity);
        entry.State = EntityState.Modified;
        return Task.CompletedTask;
    }
}
using CrowdFest.API.Abstractions.Repositories;
using CrowdFest.API.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace CrowdFest.API.Repositories;

internal sealed class PostRepository : IPostRepository
{
    private readonly CrowdFestDbContext _context;
    public PostRepository(CrowdFestDbContext context)
    {
        _context = context;
    }

    public Task CreateAsync(PostEntity postEntity, CancellationToken cancellationToken)
    {
        return _context.posts
                .AddAsync(postEntity, cancellationToken)
                .AsTask();
    }

    public Task DeleteAsync(PostEntity postEntity, CancellationToken cancellationToken)
    {
        _context.posts.Remove(postEntity);
        return Task.CompletedTask;
    }

    public async Task<IEnumerable<PostEntity>> ListAsync(CancellationToken cancellationToken)
    {
        IEnumerable<PostEntity> entities = 
            await _context.posts
                .AsNoTracking()
                .ToListAsync();
        
        return entities;
    }

    public async Task<IEnumerable<PostEntity>> ListPlannerPostsAsync(Guid plannerId, CancellationToken cancellationToken)
    {
        IEnumerable<PostEntity> entities = 
            await _context.posts
                .AsNoTracking()
                .Where(p => p.plannerId.Equals(plannerId))
                .ToListAsync();
        
        return entities;
    }

    public Task<PostEntity?> RetrieveAsync(Guid id, Guid plannerId, CancellationToken cancellationToken)
    {
        return _context.posts
            .AsNoTracking()
            .FirstOrDefaultAsync(p => p.id.Equals(id) && p.plannerId.Equals(plannerId), cancellationToken);
    }

    public Task SaveChangesAsync(CancellationToken cancellationToken)
    {
        return _context.SaveChangesAsync();
    }

    public Task UpdateAsync(PostEntity postEntity, CancellationToken cancellationToken)
    {
        EntityEntry<PostEntity> entry = _context.posts.Entry(postEntity);
        entry.State = EntityState.Modified;
        return Task.CompletedTask;
    }
}
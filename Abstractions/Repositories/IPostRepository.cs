using CrowdFest.API.Entities;

namespace CrowdFest.API.Abstractions.Repositories;

public interface IPostRepository
{
    Task CreateAsync(PostEntity postEntity, CancellationToken cancellationToken);
    Task DeleteAsync(PostEntity postEntity, CancellationToken cancellationToken);
    Task<IEnumerable<PostEntity>> ListAsync(CancellationToken cancellationToken);
    Task<IEnumerable<PostEntity>> ListPlannerPostsAsync(Guid plannerId, CancellationToken cancellationToken);
    Task<PostEntity?> RetrieveAsync(Guid id, Guid plannerId, CancellationToken cancellationToken);
    Task UpdateAsync(PostEntity postEntity, CancellationToken cancellationToken);
    Task SaveChangesAsync(CancellationToken cancellationToken);
}
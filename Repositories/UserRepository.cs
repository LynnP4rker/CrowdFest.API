using CrowdFest.API.Abstractions.Repositories;
using CrowdFest.API.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace CrowdFest.API.Repositories;
internal sealed class UserRepository : IUserRepository
{
    private readonly CrowdFestDbContext _context;
    public UserRepository(CrowdFestDbContext context)
    {
        _context = context;
    }
    public Task CreateAsync(UserEntity userEntity, CancellationToken cancellationToken)
    {
        return _context.users
                .AddAsync(userEntity, cancellationToken)
                .AsTask();
    }

    public Task DeleteAsync(UserEntity userEntity, CancellationToken cancellationToken)
    {
        _context.users.Remove(userEntity);
        return Task.CompletedTask;
    }

    public Task<UserEntity?> RetrieveAsync(string emailAddress, CancellationToken cancellationToken)
    {
        return _context.users
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.emailAddress.Equals(emailAddress), cancellationToken);
    }

    public Task SaveChangesAsync(CancellationToken cancellationToken)
    {
        return _context.SaveChangesAsync();
    }

    public Task UpdateAsync(UserEntity userEntity, CancellationToken cancellationToken)
    {
        EntityEntry<UserEntity> entry = _context.users.Entry(userEntity);
        entry.State = EntityState.Modified;
        return Task.CompletedTask;
    }
}
using CrowdFest.API.Abstractions.Repositories;
using CrowdFest.API.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace CrowdFest.API.Repositories;

internal sealed class ThemeRepository : IThemeRepository
{
    private readonly CrowdFestDbContext _context;

    public ThemeRepository(CrowdFestDbContext context)
    {
        _context = context;
    }
    public Task CreateAsync(ThemeEntity themeEntity, CancellationToken cancellationToken)
    {
        return _context.themes.AddAsync(themeEntity, cancellationToken)
            .AsTask();
    }

    public Task DeleteAsync(ThemeEntity themeEntity, CancellationToken cancellationToken)
    {
        _context.themes.Remove(themeEntity);
        return Task.CompletedTask;
    }

    public async Task<IEnumerable<ThemeEntity>> ListAsync(CancellationToken cancellationToken)
    {
        IEnumerable<ThemeEntity> entities = 
            await _context.themes
                .AsNoTracking()
                .ToArrayAsync();
        
        return entities;
    }

    public async Task<IEnumerable<ThemeEntity>> ListPlannerThemesAsync(Guid plannerId, CancellationToken cancellationToken)
    {
        IEnumerable<ThemeEntity> entities = 
            await _context.themes
                .AsNoTracking()
                .Where(t => t.plannerId.Equals(plannerId))
                .ToArrayAsync();
        
        return entities;
    }

    public Task<ThemeEntity?> RetrieveAsync(Guid id, Guid plannerId, CancellationToken cancellationToken)
    {
        return _context.themes
            .AsNoTracking()
            .FirstOrDefaultAsync(t => t.themeId.Equals(id) && t.plannerId.Equals(plannerId), cancellationToken);
    }

    public Task SaveChangesAsync(CancellationToken cancellationToken)
    {
        return _context.SaveChangesAsync();
    }

    public Task UpdateAsync(ThemeEntity themeEntity, CancellationToken cancellationToken)
    {
        EntityEntry<ThemeEntity> entry = _context.themes.Entry(themeEntity);
        entry.State = EntityState.Modified;
        return Task.CompletedTask;
    }
}
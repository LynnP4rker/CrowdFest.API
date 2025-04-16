using CrowdFest.API.Entities;

namespace CrowdFest.API.Abstractions.Repositories;

public interface IThemeRepository
{
    Task CreateAsync(ThemeEntity themeEntity, CancellationToken cancellationToken);
    Task DeleteAsync(ThemeEntity themeEntity, CancellationToken cancellationToken);
    Task<IEnumerable<ThemeEntity>> ListAsync(CancellationToken cancellationToken);
    Task<IEnumerable<ThemeEntity>> ListPlannerThemesAsync(Guid plannerId, CancellationToken cancellationToken);
    Task<ThemeEntity?> RetrieveAsync(Guid id, Guid plannerId, CancellationToken cancellationToken);
    Task UpdateAsync(ThemeEntity themeEntity, CancellationToken cancellationToken);
    Task SaveChangesAsync(CancellationToken cancellationToken);
}
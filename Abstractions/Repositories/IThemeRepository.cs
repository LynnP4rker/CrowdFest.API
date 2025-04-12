using CrowdFest.API.Entities;

namespace CrowdFest.API.Abstractions.Repositories;

public interface IThemeRepository
{
    Task CreateAsync(ThemeEntity themeEntity, CancellationToken cancellationToken);
    Task DeleteAsync(ThemeEntity themeEntity, CancellationToken cancellationToken);
    Task<IEnumerable<ThemeEntity>> ListAsync(CancellationToken cancellationToken);
    Task<ThemeEntity> RetrieveAsync(int id, CancellationToken cancellationToken);
    Task UpdateAsync(ThemeEntity themeEntity, CancellationToken cancellationToken);
}
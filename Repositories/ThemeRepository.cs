using CrowdFest.API.Abstractions.Repositories;
using CrowdFest.API.Entities;

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
        throw new NotImplementedException();
    }

    public Task DeleteAsync(ThemeEntity themeEntity, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<ThemeEntity>> ListAsync(CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<ThemeEntity> RetrieveAsync(int id, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task UpdateAsync(ThemeEntity themeEntity, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
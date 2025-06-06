using CrowdFest.API.Abstractions.Repositories;
using CrowdFest.API.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

internal sealed class OrganizationRepository : IOrganizationRepository
{
    private readonly CrowdFestDbContext _context;
    public OrganizationRepository(CrowdFestDbContext context)
    {
        _context = context;
    }

    public Task CreateAsync(OrganizationEntity organisationEntity, CancellationToken cancellationToken)
    {
        return _context.organisations
                .AddAsync(organisationEntity, cancellationToken)
                .AsTask();
    }

    public Task DeleteAsync(OrganizationEntity organisationEntity, CancellationToken cancellationToken)
    {
        _context.organisations.Remove(organisationEntity);
        return Task.CompletedTask;
    }

    public async Task<IEnumerable<OrganizationEntity>> ListAsync(CancellationToken cancellationToken)
    {
        return await _context.organisations
                    .AsNoTracking()
                    .ToArrayAsync(cancellationToken);
    }

    public Task<OrganizationEntity?> RetrieveAsync(Guid id, CancellationToken cancellationToken)
    {
        return _context.organisations
                    .AsNoTracking()
                    .FirstOrDefaultAsync(o => o.id.Equals(id), cancellationToken);
    }

    public Task<OrganizationEntity?> RetrieveEmailAsync(string emailAddress, CancellationToken cancellationToken)
    {
        return _context.organisations
                    .AsNoTracking()
                    .FirstOrDefaultAsync(o => o.emailAddress.Equals(emailAddress), cancellationToken);
    }

    public Task SaveChangesAsync(CancellationToken cancellationToken)
    {
        return _context.SaveChangesAsync(cancellationToken);
    }

    public Task UpdateAsync(OrganizationEntity organizationEntity, CancellationToken cancellationToken)
    {
        EntityEntry<OrganizationEntity> entry = _context.organisations.Entry(organizationEntity);
        entry.State = EntityState.Modified;
        return Task.CompletedTask;
    }
}
using CrowdFest.API.Abstractions.Repositories;
using CrowdFest.API.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace CrowdFest.API.Repositories;

internal sealed class OrganizationAccountRepository : IOrganizationAccountRepository
{
    private readonly CrowdFestDbContext _context;
    public OrganizationAccountRepository(CrowdFestDbContext context)
    {
        _context = context;
    }

    public Task CreateAsync(OrganizationAccountEntity organisationAccountEntity, CancellationToken cancellationToken)
    {
        return _context.organisationAccounts
            .AddAsync(organisationAccountEntity, cancellationToken)
            .AsTask();
    }

    public Task DeleteAsync(OrganizationAccountEntity organisationAccountEntity, CancellationToken cancellationToken)
    {
        _context.organisationAccounts.Remove(organisationAccountEntity);
        return Task.CompletedTask;
        
    }

    public Task<OrganizationAccountEntity?> RetrieveAsync(Guid id, CancellationToken cancellationToken)
    {
        return _context.organisationAccounts
                    .AsNoTracking()
                    .FirstOrDefaultAsync(o => o.id.Equals(id), cancellationToken);
    }

    public Task SaveChangesAsync(CancellationToken cancellationToken)
    {
        return _context.SaveChangesAsync(cancellationToken);
    }

    public Task UpdateAsync(OrganizationAccountEntity organisationAccountEntity, CancellationToken cancellationToken)
    {
        EntityEntry<OrganizationAccountEntity> entry = _context.organisationAccounts.Entry(organisationAccountEntity);
        entry.State = EntityState.Modified;
        return Task.CompletedTask;
    }
}
using AutoMapper;
using CrowdFest.API.Abstractions.Repositories;
using CrowdFest.API.Entities;
using CrowdFest.API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CrowdFest.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class OrganizationController: ControllerBase
{
    private readonly IMapper _mapper;
    private readonly IOrganizationRepository _repository;
    private readonly ILogger<OrganizationEntity> _logger;

    public OrganizationController(IMapper mapper, IOrganizationRepository repository, ILogger<OrganizationEntity> logger)
    {
        _mapper = mapper;
        _repository = repository;
        _logger = logger;
    }

    [HttpGet("{id}")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<OrganizationDto>> RetrieveOrganizationAsync(Guid id, CancellationToken cancellationToken)
    {
        try 
        {
            OrganizationEntity? organizationEntity = await _repository.RetrieveAsync(id, cancellationToken);
            if (organizationEntity is null)
            {
                _logger.LogInformation($"Unable to retrieve organization id: {id}");
                return NotFound();
            }
            OrganizationDto organisation = _mapper.Map<OrganizationDto>(organizationEntity);
            return Ok(organisation);
        } catch (Exception ex)
        {
            _logger.LogError($"{ex}");
            return StatusCode(500, "A problem happened while trying to process your request");
        }
    }

    [HttpGet]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<IEnumerable<OrganizationDto>>> ListOrganizationsAsync(CancellationToken cancellationToken)
    {
        try
        {
            IEnumerable<OrganizationEntity> organizationEntities = await _repository.ListAsync(cancellationToken);
            IEnumerable<OrganizationDto> organisations = _mapper.Map<IEnumerable<OrganizationDto>>(organizationEntities);
            return Ok(organisations);
        } catch (Exception ex)
        {
            _logger.LogError(ex, "Unable to list organisations");
            return StatusCode(500, "A problem happened while trying to process your request");
        }
    }

    [HttpDelete("{id}")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> RemoveOrganizationAsync(Guid id, CancellationToken cancellationToken)
    {
        try 
        {
            OrganizationEntity? organizationEntity = await _repository.RetrieveAsync(id, cancellationToken);
            if (organizationEntity is null)
            {
                _logger.LogInformation($"Unable to retrieve organization id: {id}");
                return NotFound();
            }

            await _repository.DeleteAsync(organizationEntity, cancellationToken);
            await _repository.SaveChangesAsync(cancellationToken);
            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError($"Unable to remove organization with organization id: {id}", ex);
            return StatusCode(500, "A problem happened while trying to process your request");
        }
    }

    [HttpPut("{id}")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> UpdateOrganizationAsync(Guid id, [FromBody] OrganizationDto organisation, CancellationToken cancellationToken)
    {
        try
        {
            if (id != organisation.id) { return BadRequest ("Id in URL doesn't match the body"); }
            OrganizationEntity? organizationEntity = await _repository.RetrieveAsync(id, cancellationToken);
            if (organizationEntity is null) { return NotFound(); }
            _mapper.Map(organisation, organizationEntity);
            await _repository.UpdateAsync(organizationEntity, cancellationToken);
            await _repository.SaveChangesAsync(cancellationToken);
            return NoContent();
        } catch (Exception ex)
        {
             _logger.LogError($"Unable to update organisation with organisation id: {id}", ex);
            return StatusCode(500, "A problem happened while trying to process your request");     
        }
    }
}
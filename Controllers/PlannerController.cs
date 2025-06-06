using AutoMapper;
using CrowdFest.API.Abstractions.Repositories;
using CrowdFest.API.Entities;
using CrowdFest.API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CrowdFest.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[ProducesResponseType(StatusCodes.Status500InternalServerError)]
public class PlannerController: ControllerBase
{
    private readonly ILogger<PlannerController> _logger;
    private readonly IPlannerRepository _repository;
    private readonly IPlannerAccountRepository _account;
    private readonly IPasswordService _password;
    private readonly IMapper _mapper;
    public PlannerController(
        IPlannerRepository repository, 
        IPlannerAccountRepository account, 
        IMapper mapper, 
        ILogger<PlannerController> logger, 
        IPasswordService password
        )
    {
        _repository = repository
            ?? throw new ArgumentNullException(nameof(repository));
        _account = account
            ?? throw new ArgumentNullException(nameof(account));
        _mapper = mapper
            ?? throw new ArgumentNullException(nameof(mapper));
        _logger = logger
            ?? throw new ArgumentNullException(nameof(logger));
        _password = password
            ?? throw new ArgumentNullException(nameof(logger));
    }

    [HttpGet("{id}")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<PlannerDto>> RetrievePlannerAsync(Guid id, CancellationToken cancellationToken)
    {
        try 
        {
            PlannerEntity? plannerEntity = await _repository.RetrieveAsync(id, cancellationToken);
            if(plannerEntity is null) 
            {
                _logger.LogInformation($"Unable to retrieve planner id: {id}");
                return NotFound();
            }
            PlannerDto planner = _mapper.Map<PlannerDto>(plannerEntity);
            return Ok(planner);
        } catch (Exception ex)
        {
            _logger.LogError($"Unable to retrieve planner with planner id: {id}", ex);
            return StatusCode(500, "A problem happened while trying to process your request");
        }
    }

    [HttpGet("{emailaddress}")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<PlannerDto>> RetrievePlannerFromEmailAddressAsync(string emailaddress, CancellationToken cancellationToken)
    {
        try 
        {
            PlannerEntity? plannerEntity = await _repository.RetrieveEmailAsync(emailaddress, cancellationToken);
            if (plannerEntity is null)
            {
                _logger.LogInformation($"Unable to retrieve planner emailaddress: {emailaddress}");
                return NotFound();
            }
            PlannerDto planner = _mapper.Map<PlannerDto>(plannerEntity);
            return Ok(planner);
        } catch (Exception ex)
        {
            _logger.LogError($"Unable to retrieve planner with planner id: {emailaddress}", ex);
            return StatusCode(500, "A problem happened while trying to process your request");
        }
    }

    [HttpGet]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<IEnumerable<PlannerDto>>> ListPlannersAsync(CancellationToken cancellationToken)
    {
        try 
        {
            IEnumerable<PlannerEntity> plannerEntities = await _repository.ListAsync(cancellationToken);
            IEnumerable<PlannerDto> planners = _mapper.Map<IEnumerable<PlannerDto>>(plannerEntities);
            return Ok(planners);
        } catch (Exception ex)
        {
            _logger.LogError(ex, "Unable to list planners");
            return StatusCode(500, "A problem happened while trying to process your request");
        }
    }

    [HttpDelete("{id}")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> RemovePlannerAsync(Guid id, CancellationToken cancellationToken)
    {
        try {

            PlannerEntity? plannerEntity = await _repository.RetrieveAsync(id, cancellationToken);
            if (plannerEntity is null) 
            {
                _logger.LogInformation($"Unable to retrieve planner id: {id}");
                return NotFound();
            }
            await _repository.DeleteAsync(plannerEntity, cancellationToken);
            await _repository.SaveChangesAsync(cancellationToken);
            return NoContent();

        } catch (Exception ex)
        {
            _logger.LogError($"Unable to remove planner with planner id: {id}", ex);
            return StatusCode(500, "A problem happened while trying to process your request");
        }
    }

    [HttpPut("{id}")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> UpdatePlannerAsync(Guid id, [FromBody] PlannerDto planner, CancellationToken cancellationToken)
    {
        try 
        {
            if (id != planner.id) { return BadRequest ("ID in URL doesn't match the body"); }
            PlannerEntity? plannerEntity = await _repository.RetrieveAsync(id, cancellationToken);
            if (plannerEntity is null) { return NotFound(); }
            _mapper.Map(planner, plannerEntity);
            await _repository.UpdateAsync(plannerEntity, cancellationToken);
            await _repository.SaveChangesAsync(cancellationToken);
            return NoContent();
        } catch (Exception ex)
        {
            _logger.LogError($"Unable to update planner with planner id: {id}", ex);
            return StatusCode(500, "A problem happened while trying to process your request");
        }
    }
}
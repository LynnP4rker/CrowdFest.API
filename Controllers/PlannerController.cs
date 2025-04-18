using AutoMapper;
using CrowdFest.API.Abstractions.Repositories;
using CrowdFest.API.Entities;
using CrowdFest.API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CrowdFest.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
[ProducesResponseType(StatusCodes.Status401Unauthorized)]
[ProducesResponseType(StatusCodes.Status500InternalServerError)]
public class PlannerController: ControllerBase
{
    private readonly ILogger<PlannerController> _logger;
    private readonly IPlannerRepository _repository;
    private readonly IMapper _mapper;
    public PlannerController(IPlannerRepository repository, IMapper mapper, ILogger<PlannerController> logger)
    {
        _repository = repository
            ?? throw new ArgumentNullException(nameof(repository));
        _mapper = mapper
            ?? throw new ArgumentNullException(nameof(mapper));
        _logger = logger
            ?? throw new ArgumentNullException(nameof(logger));
    }

    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<PlannerDto>> RetrievePlannerAsync(Guid id, CancellationToken cancellationToken)
    {
        try 
        {
            PlannerEntity? plannerEntity = await _repository.RetrieveAsync(id, cancellationToken);
            if(plannerEntity is null) 
            {
                _logger.LogInformation($"Unable to retrieve location id: {id}");
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

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
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

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreatePlannerAsync([FromBody] PlannerDto planner, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid) 
        { 
            return BadRequest(ModelState); 
        }
        try 
        {
            PlannerEntity plannerEntity = _mapper.Map<PlannerEntity>(planner);
            await _repository.CreateAsync(plannerEntity, cancellationToken);
            await _repository.SaveChangesAsync(cancellationToken);
            PlannerDto plannerDto = _mapper.Map<PlannerDto>(plannerEntity);
            return CreatedAtAction(nameof(RetrievePlannerAsync), new { id = plannerEntity.id}, plannerDto);
        } catch (Exception ex)
        {
            _logger.LogError($"Unable to create planner: {planner}", ex);
            return StatusCode(500, "A problem happened while trying to process your request");
        }
    }

    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
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
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
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
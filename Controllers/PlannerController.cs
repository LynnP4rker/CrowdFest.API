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
public class PlannerController: ControllerBase
{
    private readonly IPlannerRepository _repository;
    private readonly IMapper _mapper;
    public PlannerController(IPlannerRepository repository, IMapper mapper)
    {
        _repository = repository
            ?? throw new ArgumentNullException(nameof(repository));
        _mapper = mapper
            ?? throw new ArgumentNullException(nameof(mapper));
    }

    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<PlannerDto>> RetrievePlannerAsync(Guid id, CancellationToken cancellationToken)
    {
        PlannerEntity? plannerEntity = await _repository.RetrieveAsync(id, cancellationToken);
        if(plannerEntity is null) return NotFound();
        PlannerDto planner = _mapper.Map<PlannerDto>(plannerEntity);
        return Ok(planner);
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<PlannerDto>>> ListPlannersAsync(CancellationToken cancellationToken)
    {
        IEnumerable<PlannerEntity> plannerEntities = await _repository.ListAsync(cancellationToken);
        IEnumerable<PlannerDto> planners = _mapper.Map<IEnumerable<PlannerDto>>(plannerEntities);
        return Ok(planners);
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public async Task<IActionResult> CreatePlannerAsync([FromBody] PlannerDto planner, CancellationToken cancellationToken)
    {
        PlannerEntity plannerEntity = _mapper.Map<PlannerEntity>(planner);
        await _repository.CreateAsync(plannerEntity, cancellationToken);
        await _repository.SaveChangesAsync(cancellationToken);
        PlannerDto plannerDto = _mapper.Map<PlannerDto>(plannerEntity);
        return CreatedAtAction(nameof(RetrievePlannerAsync), new { id = plannerEntity.id}, plannerDto);
    }

    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> RemovePlannerAsync(Guid id, CancellationToken cancellationToken)
    {
        PlannerEntity? plannerEntity = await _repository.RetrieveAsync(id, cancellationToken);
        if (plannerEntity is null) return NotFound();
        await _repository.DeleteAsync(plannerEntity, cancellationToken);
        await _repository.SaveChangesAsync(cancellationToken);
        return NoContent();
    }

    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> UpdatePlannerAsync(Guid id, [FromBody] PlannerDto planner, CancellationToken cancellationToken)
    {
        if (id != planner.id) { return BadRequest ("ID in URL doesn't match the body"); }
        PlannerEntity? plannerEntity = await _repository.RetrieveAsync(id, cancellationToken);
        if (plannerEntity is null) { return NotFound(); }
        _mapper.Map(planner, plannerEntity);
        await _repository.UpdateAsync(plannerEntity, cancellationToken);
        await _repository.SaveChangesAsync(cancellationToken);
        return NoContent();
    }
}
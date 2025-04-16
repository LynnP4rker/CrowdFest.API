using AutoMapper;
using CrowdFest.API.Abstractions.Repositories;
using CrowdFest.API.Entities;
using CrowdFest.API.Models;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class PlannerGroupController: ControllerBase
{   
    private readonly IPlannerGroupRepository _repository;
    private readonly IMapper _mapper;

    public PlannerGroupController(IMapper mapper, IPlannerGroupRepository repository)
    {
        _repository = repository;
        _mapper = mapper;
    }

    [HttpGet("{plannerId:guid}/{groupId:guid}")]
    public async Task<ActionResult<PlannerGroupDto>> RetrieveGroupForPlannerAsync(Guid plannerId, Guid groupId, CancellationToken cancellationToken)
    {
        PlannerGroupEntity? plannerGroupEntity = await _repository.RetrieveAsync(plannerId, groupId, cancellationToken);
        if (plannerGroupEntity is null) return NotFound();
        PlannerGroupDto plannerGroup = _mapper.Map<PlannerGroupDto>(plannerGroupEntity);
        return Ok(plannerGroup);
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<PlannerGroupDto>>> ListGroupsForPlannerAsync(Guid plannerId, CancellationToken cancellationToken)
    {
        IEnumerable<PlannerGroupEntity> plannerGroupEntities = await _repository.ListAsync(plannerId, cancellationToken);
        IEnumerable<PlannerGroupDto> plannerGroups = _mapper.Map<IEnumerable<PlannerGroupDto>>(plannerGroupEntities);
        return Ok(plannerGroups);
    }

    [HttpPost]
    public async Task<IActionResult> CreatePlannerGroupAsync([FromBody] PlannerGroupDto plannerGroup, CancellationToken cancellationToken)
    {
        PlannerGroupEntity plannerGroupEntity = _mapper.Map<PlannerGroupEntity>(plannerGroup);
        await _repository.CreateAsync(plannerGroupEntity, cancellationToken);
        await _repository.SaveChangesAsync(cancellationToken);
        PlannerGroupDto returnPlannerGroup = _mapper.Map<PlannerGroupDto>(plannerGroupEntity);
        return CreatedAtAction(nameof(RetrieveGroupForPlannerAsync), new { id = plannerGroupEntity.plannerId}, returnPlannerGroup);
    }

    [HttpDelete("{plannerId:guid}/{groupId:guid}")]
    public async Task<IActionResult> DeletePlannerFromGroup(Guid plannerId, Guid groupId, CancellationToken cancellationToken)
    {
        PlannerGroupEntity? plannerGroupEntity = await _repository.RetrieveAsync(plannerId, groupId, cancellationToken);
        if (plannerGroupEntity is null) return NotFound();
        await _repository.DeleteAsync(plannerGroupEntity, cancellationToken);
        await _repository.SaveChangesAsync(cancellationToken);
        return NoContent();
    }
}
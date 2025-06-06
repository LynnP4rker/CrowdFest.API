using AutoMapper;
using CrowdFest.API.Abstractions.Repositories;
using CrowdFest.API.Entities;
using CrowdFest.API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
[Authorize]
[ProducesResponseType(StatusCodes.Status401Unauthorized)]
[ProducesResponseType(StatusCodes.Status500InternalServerError)]
public class PlannerGroupController: ControllerBase
{   
    private readonly ILogger<PlannerGroupController> _logger;
    private readonly IPlannerGroupRepository _repository;
    private readonly IMapper _mapper;

    public PlannerGroupController(IMapper mapper, IPlannerGroupRepository repository, ILogger<PlannerGroupController> logger)
    {
        _repository = repository
            ?? throw new ArgumentNullException(nameof(repository));
        _mapper = mapper
            ?? throw new ArgumentNullException(nameof(mapper));
        _logger = logger
            ?? throw new ArgumentNullException(nameof(logger));
    }

    [HttpGet("{plannerId:guid}/{groupId:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<PlannerGroupDto>> RetrieveGroupForPlannerAsync(Guid plannerId, Guid groupId, CancellationToken cancellationToken)
    {
        try 
        {

            PlannerGroupEntity? plannerGroupEntity = await _repository.RetrieveAsync(plannerId, groupId, cancellationToken);
            if (plannerGroupEntity is null) 
            {
                _logger.LogInformation($"Unable to retrieve planner id: {plannerId} and group id: {groupId}");
                return NotFound();
            }
            PlannerGroupDto plannerGroup = _mapper.Map<PlannerGroupDto>(plannerGroupEntity);
            return Ok(plannerGroup);

        } catch (Exception ex)
        {   
            _logger.LogError($"Unable to retrieve planner group, plannerId: {plannerId} groupId: {groupId}", ex);
            return StatusCode(500, "A problem happened while trying to process your request");
        }   
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<PlannerGroupDto>>> ListGroupsForPlannerAsync(Guid plannerId, CancellationToken cancellationToken)
    { 
        try 
        {
            IEnumerable<PlannerGroupEntity> plannerGroupEntities = await _repository.ListAsync(plannerId, cancellationToken);
            IEnumerable<PlannerGroupDto> plannerGroups = _mapper.Map<IEnumerable<PlannerGroupDto>>(plannerGroupEntities);
            return Ok(plannerGroups);
        } catch (Exception ex)
        {
            _logger.LogError($"Unable to retreieve planner group, plannerId: {plannerId}", ex);
            return StatusCode(500, "A problem happened while trying to process your request");
        }
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreatePlannerGroupAsync([FromBody] PlannerGroupDto plannerGroup, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        try 
        {
            PlannerGroupEntity plannerGroupEntity = _mapper.Map<PlannerGroupEntity>(plannerGroup);
            await _repository.CreateAsync(plannerGroupEntity, cancellationToken);
            await _repository.SaveChangesAsync(cancellationToken);
            PlannerGroupDto returnPlannerGroup = _mapper.Map<PlannerGroupDto>(plannerGroupEntity);
            return CreatedAtAction(nameof(RetrieveGroupForPlannerAsync), new { id = plannerGroupEntity.plannerId}, returnPlannerGroup);
        } catch (Exception ex) 
        {
            _logger.LogError($"Unable to create planner group: {plannerGroup}", ex);
            return StatusCode(500, "A problem happened while trying to process your request");
        }
    }

    [HttpDelete("{plannerId:guid}/{groupId:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> DeletePlannerFromGroup(Guid plannerId, Guid groupId, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest();
        }
        try 
        {
            PlannerGroupEntity? plannerGroupEntity = await _repository.RetrieveAsync(plannerId, groupId, cancellationToken);
            if (plannerGroupEntity is null) 
            {
                _logger.LogInformation($"Unable to retrieve planner id: {plannerId} and group id: {groupId}");
                return NotFound();
            }
            await _repository.DeleteAsync(plannerGroupEntity, cancellationToken);
            await _repository.SaveChangesAsync(cancellationToken);
            return NoContent();
        } catch (Exception ex)
        {
            _logger.LogError($"Unable to delete planner id: {plannerId} group id: {groupId}", ex);
            return StatusCode(500, "A problem happened while trying to process your request");
        }
    }
}
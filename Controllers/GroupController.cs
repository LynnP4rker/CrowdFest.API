using AutoMapper;
using CrowdFest.API.Abstractions.Repositories;
using CrowdFest.API.Entities;
using CrowdFest.API.Models;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class GroupController: ControllerBase
{
    private readonly IGroupRepository _repository;
    private readonly IMapper _mapper;

    public GroupController(IGroupRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<GroupDto>> RetrieveGroupAsync(Guid id, CancellationToken cancellationToken)
    {
        GroupEntity? groupEntity = await _repository.RetrieveAsync(id, cancellationToken);
        if(groupEntity is null) return NotFound();
        GroupDto group = _mapper.Map<GroupDto>(groupEntity);
        return Ok(group);
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<GroupDto>>> ListGroupsAsync(CancellationToken cancellationToken)
    {
        IEnumerable<GroupEntity> groupEntities = await _repository.ListAsync(cancellationToken);
        IEnumerable<GroupDto> groups = _mapper.Map<IEnumerable<GroupDto>>(groupEntities);
        return Ok(groups);
    }

    [HttpPost]
    public async Task<IActionResult> CreateGroupAsync([FromBody] GroupDto group, CancellationToken cancellationToken)
    {
        GroupEntity groupEntity = _mapper.Map<GroupEntity>(group);
        await _repository.CreateAsync(groupEntity, cancellationToken);
        await _repository.SaveChangesAsync(cancellationToken);
        GroupDto returnedGroup = _mapper.Map<GroupDto>(groupEntity);
        return CreatedAtAction(nameof(RetrieveGroupAsync), new { id = groupEntity.id}, returnedGroup);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteGroupAsync(Guid id, CancellationToken cancellationToken)
    {
        GroupEntity? groupEntity = await _repository.RetrieveAsync(id, cancellationToken);
        if(groupEntity is null) return NotFound();
        await _repository.DeleteAsync(groupEntity, cancellationToken);
        await _repository.SaveChangesAsync(cancellationToken);
        return NoContent();
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateGroupAsync(Guid id, [FromBody] GroupDto group, CancellationToken cancellationToken)
    {
        if (id != group.id) { return BadRequest("ID in URL doesn't match the body"); }
        GroupEntity? groupEntity = await _repository.RetrieveAsync(id, cancellationToken);
        if (groupEntity is null) return NotFound();
        _mapper.Map(group, groupEntity);
        await _repository.UpdateAsync(groupEntity, cancellationToken);
        await _repository.SaveChangesAsync(cancellationToken);
        return NoContent();
    }

}
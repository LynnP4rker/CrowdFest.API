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
public class GroupController: ControllerBase
{
    private readonly ILogger<GroupController> _logger;
    private readonly IGroupRepository _repository;
    private readonly IMapper _mapper;

    public GroupController(IGroupRepository repository, IMapper mapper, ILogger<GroupController> logger)
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
    public async Task<ActionResult<GroupDto>> RetrieveGroupAsync(Guid id, CancellationToken cancellationToken)
    {
        try {

            GroupEntity? groupEntity = await _repository.RetrieveAsync(id, cancellationToken);
            if(groupEntity is null) 
            {
                _logger.LogInformation($"Unable to retrieve group id: {id}");
                return NotFound();
            }
            GroupDto group = _mapper.Map<GroupDto>(groupEntity);
            return Ok(group);

        } catch (Exception ex) {
            _logger.LogError($"Unable to retrieve group with group id: {id}", ex);
            return StatusCode(500, "A problem happened while trying to process your request");
        }

    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<GroupDto>>> ListGroupsAsync(CancellationToken cancellationToken)
    {
        try {
            IEnumerable<GroupEntity> groupEntities = await _repository.ListAsync(cancellationToken);
            IEnumerable<GroupDto> groups = _mapper.Map<IEnumerable<GroupDto>>(groupEntities);
            return Ok(groups);
        } catch (Exception ex)
        {
            _logger.LogError(ex, "Unable to retrieve groups list");
            return StatusCode(500, "A problem happened while trying to process your request");
        }

    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateGroupAsync([FromBody] GroupDto group, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid) 
        { 
            return BadRequest(ModelState); 
        }
        try {
            GroupEntity groupEntity = _mapper.Map<GroupEntity>(group);
            Random rnd = new Random();

#warning use alphanumermical values would be better
            int randomNumber = rnd.Next(000000, 999999);

            groupEntity.AccessCode = randomNumber.ToString();
            await _repository.CreateAsync(groupEntity, cancellationToken);
            await _repository.SaveChangesAsync(cancellationToken);
            return Ok();
        } catch (Exception ex)
        {
            _logger.LogError($"Unable to create group {group}", ex);
            return StatusCode(500, "A problem happened while trying to process your request");
        }
    }

    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteGroupAsync(Guid id, CancellationToken cancellationToken)
    {
        try {

            GroupEntity? groupEntity = await _repository.RetrieveAsync(id, cancellationToken);
            if(groupEntity is null) 
            {
                _logger.LogInformation($"Unable to retrieve group id: {id}");
                return NotFound();
            }
            await _repository.DeleteAsync(groupEntity, cancellationToken);
            await _repository.SaveChangesAsync(cancellationToken);
            return NoContent();

        } catch (Exception ex)
        {
            _logger.LogError($"Unable to delete group with group id: {id}", ex);
            return StatusCode(500, "A problem happened while trying to process your request");
        }

    }

    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> UpdateGroupAsync(Guid id, [FromBody] GroupDto group, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid) 
        { 
            return BadRequest(ModelState); 
        }
        try {
            if (id != group.id) { return BadRequest("ID in URL doesn't match the body"); }
            GroupEntity? groupEntity = await _repository.RetrieveAsync(id, cancellationToken);
            if (groupEntity is null) 
            {
                _logger.LogInformation($"Unable to retrieve group id: {id}");
                return NotFound();
            }

            _mapper.Map(group, groupEntity);
            await _repository.UpdateAsync(groupEntity, cancellationToken);
            await _repository.SaveChangesAsync(cancellationToken);
            return NoContent();
        } catch (Exception ex) 
        {
            _logger.LogError($"Unable to update group with group id: {id}", ex);
            return StatusCode(500, "A problem happened while trying to process your request");
        }
    }

}
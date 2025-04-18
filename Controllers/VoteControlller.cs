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
public class VoteController: ControllerBase
{
    private readonly ILogger<VoteController> _logger;
    private readonly IVoteRepository _repository;
    private readonly IMapper _mapper;

    public VoteController(IVoteRepository repository, IMapper mapper, ILogger<VoteController> logger)
    {
        _repository = repository
            ?? throw new ArgumentNullException(nameof(repository));
        _mapper = mapper
            ?? throw new ArgumentNullException(nameof(mapper));
        _logger = logger
            ?? throw new ArgumentNullException(nameof(logger));
    }

    [HttpGet("{id:guid}/{plannerId:guid}")]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<VoteDto>> RetrieveVoteAsync(Guid id, Guid plannerId, CancellationToken cancellationToken)
    {
        try
        {
            VoteEntity? voteEntity = await _repository.RetrieveAsync(id, plannerId, cancellationToken);
            if (voteEntity is null) 
            {
                _logger.LogInformation($"Unable to retrieve vote id: {id}");
                return NotFound();
            }
            VoteDto vote = _mapper.Map<VoteDto>(voteEntity);
            return Ok(vote);
        } catch (Exception ex)
        {
            _logger.LogError($"Unable to retrieve vote with vote id: {id}, planner id: {plannerId}", ex);
            return StatusCode(500, "A problem happened while trying to process your request");
        }
    }

    [HttpGet("{groupId:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<VoteDto>>> ListVotesForGroupAsync(Guid groupId, CancellationToken cancellationToken)
    {
        try
        {
            IEnumerable<VoteEntity> voteEntities = await _repository.ListVotesForGroupAsync(groupId, cancellationToken);
            IEnumerable<VoteDto> votes = _mapper.Map<IEnumerable<VoteDto>>(voteEntities);
            return Ok(votes);
        } catch (Exception ex)
        {
            _logger.LogError($"Unable to list votes for group with group id: {groupId}", ex);
            return StatusCode(500, "A problem happened while trying to process your request");
        }
    }

    [HttpGet("{plannerId:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<VoteDto>>> ListVotesForPlannerAsync(Guid plannerId, CancellationToken cancellationToken)
    {
        try
        {
            IEnumerable<VoteEntity> voteEntities = await _repository.ListVotesForPlannerAsync(plannerId, cancellationToken);
            IEnumerable<VoteDto> votes = _mapper.Map<IEnumerable<VoteDto>>(voteEntities);
            return Ok(votes);
        } catch (Exception ex)
        {
            _logger.LogError($"Unable to list votes for planner with planner id: {plannerId}", ex);
            return StatusCode(500, "A problem happened while trying to process your request");
        }
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateVoteAsync([FromBody] VoteDto vote, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        try 
        {
            VoteEntity voteEntity = _mapper.Map<VoteEntity>(vote);
            await _repository.CreateAsync(voteEntity, cancellationToken);
            await _repository.SaveChangesAsync(cancellationToken);
            VoteDto returnedVote = _mapper.Map<VoteDto>(voteEntity);
            return CreatedAtAction(nameof(RetrieveVoteAsync), new { id = voteEntity.id}, returnedVote);
        } catch (Exception ex)
        {
            _logger.LogError($"Unable to create vote, {vote}", ex);
            return StatusCode(500, "A problem happened while trying to process your request");
        }
    }

    [HttpDelete("{id:guid}/{planner:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteVoteAsync(Guid id, Guid plannerId, CancellationToken cancellationToken)
    {
        try
        {
            VoteEntity? voteEntity = await _repository.RetrieveAsync(id, plannerId, cancellationToken);
            if (voteEntity is null) 
            {
                _logger.LogInformation($"Unable to retrieve vote id: {id}");
                return NotFound();
            }
            await _repository.DeleteAsync(voteEntity, cancellationToken);
            await _repository.SaveChangesAsync(cancellationToken);
            return NoContent();
        } catch (Exception ex)
        {
            _logger.LogError($"Unable to delete vote with vote id: {id}, planner id: {plannerId}", ex);
            return StatusCode(500, "A problem happened while trying to process your request");
        }
    }

    [HttpPut("{id:guid}/{planner:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> UpdateVoteAsync(Guid id, Guid plannerId, [FromBody] VoteDto vote, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        try 
        {
            if ( plannerId != vote.plannerId && id != vote.id) return BadRequest ("ID in URL doesn't match the body"); 
            VoteEntity? voteEntity = await _repository.RetrieveAsync(id, plannerId, cancellationToken);
            if (voteEntity is null) 
            {
                _logger.LogInformation($"Unable to retrieve vote id: {id}");
                return NotFound();
            }
            _mapper.Map(vote, voteEntity);
            await _repository.UpdateAsync(voteEntity, cancellationToken);
            await _repository.SaveChangesAsync(cancellationToken);
            return NoContent();
        } catch (Exception ex)
        {
            _logger.LogError($"Unable to delete vote with vote id: {id}, planner id: {plannerId}", ex);
            return StatusCode(500, "A problem happened while trying to process your request");
        }
    }
}
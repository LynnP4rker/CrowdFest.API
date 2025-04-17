using AutoMapper;
using CrowdFest.API.Abstractions.Repositories;
using CrowdFest.API.Entities;
using CrowdFest.API.Models;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
[ProducesResponseType(StatusCodes.Status401Unauthorized)]
public class VoteController: ControllerBase
{
    private readonly IVoteRepository _repository;
    private readonly IMapper _mapper;

    public VoteController(IVoteRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    [HttpGet("{id:guid}/{planner:guid}")]
    public async Task<ActionResult<VoteDto>> RetrieveVoteAsync(Guid id, Guid plannerId, CancellationToken cancellationToken)
    {
        VoteEntity? voteEntity = await _repository.RetrieveAsync(id, plannerId, cancellationToken);
        if (voteEntity is null) return NotFound();
        VoteDto vote = _mapper.Map<VoteDto>(voteEntity);
        return Ok(vote);
    }

    [HttpGet("{groupId:guid}")]
    public async Task<ActionResult<IEnumerable<VoteDto>>> ListVotesForGroupAsync(Guid groupId, CancellationToken cancellationToken)
    {
        IEnumerable<VoteEntity> voteEntities = await _repository.ListVotesForGroupAsync(groupId, cancellationToken);
        IEnumerable<VoteDto> votes = _mapper.Map<IEnumerable<VoteDto>>(voteEntities);
        return Ok(votes);
    }

    [HttpGet("{plannerId:guid}")]
    public async Task<ActionResult<IEnumerable<VoteDto>>> ListVotesForPlannerAsync(Guid plannerId, CancellationToken cancellationToken)
    {
        IEnumerable<VoteEntity> voteEntities = await _repository.ListVotesForPlannerAsync(plannerId, cancellationToken);
        IEnumerable<VoteDto> votes = _mapper.Map<IEnumerable<VoteDto>>(voteEntities);
        return Ok(votes);
    }

    [HttpPost]
    public async Task<IActionResult> CreateVoteAsync([FromBody] VoteDto vote, CancellationToken cancellationToken)
    {
        VoteEntity voteEntity = _mapper.Map<VoteEntity>(vote);
        await _repository.CreateAsync(voteEntity, cancellationToken);
        await _repository.SaveChangesAsync(cancellationToken);
        VoteDto returnedVote = _mapper.Map<VoteDto>(voteEntity);
        return CreatedAtAction(nameof(RetrieveVoteAsync), new { id = voteEntity.id}, returnedVote);
    }

    [HttpDelete("{id:guid}/{planner:guid}")]
    public async Task<IActionResult> DeleteVoteAsync(Guid id, Guid plannerId, CancellationToken cancellationToken)
    {
        VoteEntity? voteEntity = await _repository.RetrieveAsync(id, plannerId, cancellationToken);
        if (voteEntity is null) return NotFound();
        await _repository.DeleteAsync(voteEntity, cancellationToken);
        await _repository.SaveChangesAsync(cancellationToken);
        return NoContent();
    }

    [HttpPut("{id:guid}/{planner:guid}")]
    public async Task<IActionResult> UpdateVoteAsync(Guid id, Guid plannerId, [FromBody] VoteDto vote, CancellationToken cancellationToken)
    {
        if ( plannerId != vote.plannerId && id != vote.id) return BadRequest ("ID in URL doesn't match the body"); 
        VoteEntity? voteEntity = await _repository.RetrieveAsync(id, plannerId, cancellationToken);
        if (voteEntity is null) return NotFound();
        _mapper.Map(vote, voteEntity);
        await _repository.UpdateAsync(voteEntity, cancellationToken);
        await _repository.SaveChangesAsync(cancellationToken);
        return NoContent();
    }
}
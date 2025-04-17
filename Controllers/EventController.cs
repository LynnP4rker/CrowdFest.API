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
public class EventController: ControllerBase
{
    private readonly IEventRepository _repository;
    private readonly IMapper _mapper;

    public EventController(IEventRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<EventDto>> RetrieveEventAsync(Guid id, CancellationToken cancellationToken)
    {   
        EventEntity? eventEntity = await _repository.RetrieveAsync(id, cancellationToken);
        if (eventEntity is null) return NotFound();
        EventDto eventDto = _mapper.Map<EventDto>(eventEntity);
        return Ok(eventDto);
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<EventDto>>> ListEventsAsync(CancellationToken cancellationToken)
    {
        IEnumerable<EventEntity> eventEntities = await _repository.ListAsync(cancellationToken);
        IEnumerable<EventDto> events = _mapper.Map<IEnumerable<EventDto>>(eventEntities);
        return Ok(events);
    }

    [HttpPost]
    public async Task<IActionResult> CreateEventAsync([FromBody] EventDto eventDto, CancellationToken cancellationToken)
    {
        EventEntity eventEntity = _mapper.Map<EventEntity>(eventDto);
        await _repository.CreateAsync(eventEntity, cancellationToken);
        await _repository.SaveChangesAsync(cancellationToken);
        EventDto returnedEvent = _mapper.Map<EventDto>(eventEntity);
        return CreatedAtAction(nameof(RetrieveEventAsync), new { id = eventEntity.id}, returnedEvent);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> RemoveEventAsync(Guid id, CancellationToken cancellationToken)
    {
        EventEntity? eventEntity = await _repository.RetrieveAsync(id, cancellationToken);
        if (eventEntity is null) return NotFound();
        await _repository.DeleteAsync(eventEntity, cancellationToken);
        return NoContent();
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateEventAsync(Guid id, [FromBody] EventDto eventDto, CancellationToken cancellationToken)
    {
        if (id != eventDto.id) { return BadRequest ("ID in URL doesn't match the body"); }
        EventEntity? eventEntity = await _repository.RetrieveAsync(id, cancellationToken);
        if (eventEntity is null) return NotFound();
        _mapper.Map(eventDto, eventEntity);
        await _repository.UpdateAsync(eventEntity, cancellationToken);
        await _repository.SaveChangesAsync(cancellationToken);
        return NoContent();
    }
}
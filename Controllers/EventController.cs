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
public class EventController: ControllerBase
{
    private readonly ILogger<EventController> _logger;
    private readonly IEventRepository _repository;
    private readonly IMapper _mapper;

    public EventController(IEventRepository repository, IMapper mapper, ILogger<EventController> logger)
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
    public async Task<ActionResult<EventDto>> RetrieveEventAsync(Guid id, CancellationToken cancellationToken)
    {   try {
            EventEntity? eventEntity = await _repository.RetrieveAsync(id, cancellationToken);

            if (eventEntity is null) { 
                _logger.LogInformation($"Unable to retrieve event of event id: {id}"); 
                return NotFound(); 
            }

            EventDto eventDto = _mapper.Map<EventDto>(eventEntity);
            return Ok(eventDto);

        } catch (Exception ex) { 
            _logger.LogError(
                $"Exception while getting event of event id: {id}", ex
            );
            return StatusCode(500, "A problem happened while trying to process your request");
        }
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<EventDto>>> ListEventsAsync(CancellationToken cancellationToken)
    {
        try {
            IEnumerable<EventEntity> eventEntities = await _repository.ListAsync(cancellationToken);
            IEnumerable<EventDto> events = _mapper.Map<IEnumerable<EventDto>>(eventEntities);
            return Ok(events);
        } catch (Exception ex) {
            _logger.LogError(
                ex, "Exception while getting events"
            );
            return StatusCode(500, "A problem happened while trying to process your request");
        }
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateEventAsync([FromBody] EventDto eventDto, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid) 
        { 
            return BadRequest(ModelState); 
        }
        try {
            EventEntity eventEntity = _mapper.Map<EventEntity>(eventDto);
            await _repository.CreateAsync(eventEntity, cancellationToken);
            await _repository.SaveChangesAsync(cancellationToken);
            EventDto returnedEvent = _mapper.Map<EventDto>(eventEntity);
            return CreatedAtAction(nameof(RetrieveEventAsync), new { id = eventEntity.id}, returnedEvent);
        } catch (Exception ex) {
            _logger.LogError(
                $"Exception while creating event: {eventDto}", ex
            );
            return StatusCode(500, "A problem happened while trying to process your request");
        }
    }

    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> RemoveEventAsync(Guid id, CancellationToken cancellationToken)
    {
        try {

            EventEntity? eventEntity = await _repository.RetrieveAsync(id, cancellationToken);
            if (eventEntity is null) 
            {
                _logger.LogInformation($"Unable to retrieve event of event id: {id}");
                return NotFound();
            }

            await _repository.DeleteAsync(eventEntity, cancellationToken);
            return NoContent();

        } catch (Exception ex) {
            _logger.LogError(
                $"Exception while deleting event with event id: {id}", ex
            );
            return StatusCode(500, "A problem happened while trying to process you request");
        }
    }

    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> UpdateEventAsync(Guid id, [FromBody] EventDto eventDto, CancellationToken cancellationToken)
    {   
        if (!ModelState.IsValid) 
        { 
            return BadRequest(ModelState); 
        }
        try {
            if (id != eventDto.id) { return BadRequest ("ID in URL doesn't match the body"); }
            EventEntity? eventEntity = await _repository.RetrieveAsync(id, cancellationToken);

            if (eventEntity is null) 
            {
                _logger.LogInformation($"Unable to retrieve event of event id: {id}");
                return NotFound();
            }

            _mapper.Map(eventDto, eventEntity);
            await _repository.UpdateAsync(eventEntity, cancellationToken);
            await _repository.SaveChangesAsync(cancellationToken);
            return NoContent();

        } catch (Exception ex) {
            _logger.LogError(
                $"Exception while updating event with event id: {id}", ex
            );
            return StatusCode(500, "A problem happened while trying to process you request");
        }
    }
}
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
public class LocationController: ControllerBase
{
    private readonly ILogger<LocationController> _logger;
    private readonly ILocationRepository _repository;
    private readonly IMapper _mapper;

    public LocationController(ILocationRepository repository, IMapper mapper, ILogger<LocationController> logger)
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
    public async Task<ActionResult<LocationDto>> RetrieveLocationAsync(Guid id, CancellationToken cancellationToken)
    {
        try 
        {
            LocationEntity? locationEntity = await _repository.RetrieveAsync(id, cancellationToken);
            if (locationEntity is null) 
            {
                _logger.LogInformation($"Unable to retrieve location id: {id}");
                return NotFound();
            }
            LocationDto location = _mapper.Map<LocationDto>(locationEntity);
            return Ok(location);
        } catch (Exception ex)
        {
            _logger.LogError($"Unable to retrieve location with location id {id}", ex);
            return StatusCode(500, "A problem happened while trying to process your request");
        }
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<LocationDto>>> ListLocationsAsync(CancellationToken cancellationToken)
    {
        try 
        {
            IEnumerable<LocationEntity> locationEntities = await _repository.ListAsync(cancellationToken);
            IEnumerable<LocationDto> locations = _mapper.Map<IEnumerable<LocationDto>>(locationEntities);
            return Ok(locations);
        } catch (Exception ex)
        {
            _logger.LogError(ex, "Unable to retrieve locations");
            return StatusCode(500, "A problem happened while trying to process your request");
        }
    }

    [HttpGet("planner/{id}")]
    public async Task<ActionResult<IEnumerable<LocationDto>>> ListLocationsForPlannerAsync(Guid id, CancellationToken cancellationToken)
    {
        try 
        {
            IEnumerable<LocationEntity> locationEntities = await _repository.ListLocationsForPlanner(id, cancellationToken);
            IEnumerable<LocationDto> locations = _mapper.Map<IEnumerable<LocationDto>>(locationEntities);
            return Ok(locations);
        } catch (Exception ex)
        {
            _logger.LogError(ex, "Unable to retrieve locations");
            return StatusCode(500, "A problem happened while trying to process your request");
        }
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateLocationAsync([FromBody] LocationDto location, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid) 
        { 
            return BadRequest(ModelState); 
        }
        try 
        {
            LocationEntity locationEntity = _mapper.Map<LocationEntity>(location);
            await _repository.CreateAsync(locationEntity, cancellationToken);
            await _repository.SaveChangesAsync(cancellationToken);

            LocationDto locationDto = _mapper.Map<LocationDto>(locationEntity);
            return Ok(locationDto.locationId);
            
        } catch (Exception ex)
        {
            _logger.LogError($"Unable to create location: {location}", ex);
            return StatusCode(500, "A problem happened while trying to process your request");
        }
    }

    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> RemoveLocationAsync(Guid id, CancellationToken cancellationToken)
    {
        try 
        {
            LocationEntity? locationEntity = await _repository.RetrieveAsync(id, cancellationToken);
            if (locationEntity is null) 
            {
                _logger.LogInformation($"Unable to retrieve location id: {id}");
                return NotFound();
            }
            await _repository.DeleteAsync(locationEntity, cancellationToken);
            await _repository.SaveChangesAsync(cancellationToken);
            return NoContent();
        } catch (Exception ex)
        {
            _logger.LogError($"Unable to remove location with location id: {id}", ex);
            return StatusCode(500, "A problem happened while trying to process your request");
        }
    }

    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> UpdateLocationAsync(Guid id, [FromBody] LocationDto location, CancellationToken cancellationToken)
    {   if (!ModelState.IsValid) 
        { 
            return BadRequest(ModelState); 
        }
        try 
        {

            if (id != location.locationId) { return BadRequest ("ID in URL doesn't match the body"); }
            LocationEntity? locationEntity = await _repository.RetrieveAsync(id, cancellationToken);
            if (locationEntity is null) 
            {
                _logger.LogInformation($"Unable to retrieve location id: {id}");
                return NotFound();
            }
            _mapper.Map(location, locationEntity);
            await _repository.UpdateAsync(locationEntity, cancellationToken);
            await _repository.SaveChangesAsync(cancellationToken);
            return NoContent();

        } catch (Exception ex)
        {
            _logger.LogError($"Unable to update location with location id: {id}", ex);
            return StatusCode(500, "A problem happened while trying to process your request");
        }
    }

}
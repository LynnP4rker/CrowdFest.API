using AutoMapper;
using CrowdFest.API.Abstractions.Repositories;
using CrowdFest.API.Entities;
using CrowdFest.API.Models;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class LocationController: ControllerBase
{
    private readonly ILocationRepository _repository;
    private readonly IMapper _mapper;

    public LocationController(ILocationRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<LocationDto>> RetrieveLocationAsync(Guid id, CancellationToken cancellationToken)
    {
        LocationEntity? locationEntity = await _repository.RetrieveAsync(id, cancellationToken);
        if (locationEntity is null) return NotFound();
        LocationDto location = _mapper.Map<LocationDto>(locationEntity);
        return Ok(location);
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<LocationDto>>> ListLocationsAsync(CancellationToken cancellationToken)
    {
        IEnumerable<LocationEntity> locationEntities = await _repository.ListAsync(cancellationToken);
        IEnumerable<LocationDto> locations = _mapper.Map<IEnumerable<LocationDto>>(locationEntities);
        return Ok(locations);
    }

    [HttpPost]
    public async Task<IActionResult> CreateLocationAsync([FromBody] LocationDto location, CancellationToken cancellationToken)
    {
        LocationEntity locationEntity = _mapper.Map<LocationEntity>(location);
        await _repository.CreateAsync(locationEntity, cancellationToken);
        await _repository.SaveChangesAsync(cancellationToken);
        LocationDto returnedLocation = _mapper.Map<LocationDto>(locationEntity);
        return CreatedAtAction(nameof(RetrieveLocationAsync), new { id = locationEntity.locationId}, returnedLocation);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> RemoveLocationAsync(Guid id, CancellationToken cancellationToken)
    {
        LocationEntity? locationEntity = await _repository.RetrieveAsync(id, cancellationToken);
        if (locationEntity is null) return NotFound();
        await _repository.DeleteAsync(locationEntity, cancellationToken);
        await _repository.SaveChangesAsync(cancellationToken);
        return NoContent();
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateLocationAsync(Guid id, [FromBody] LocationDto location, CancellationToken cancellationToken)
    {
        if (id != location.locationId) { return BadRequest ("ID in URL doesn't match the body"); }
        LocationEntity? locationEntity = await _repository.RetrieveAsync(id, cancellationToken);
        if (locationEntity is null) return NotFound();
        _mapper.Map(location, locationEntity);
        await _repository.UpdateAsync(locationEntity, cancellationToken);
        await _repository.SaveChangesAsync(cancellationToken);
        return NoContent();
    }

}
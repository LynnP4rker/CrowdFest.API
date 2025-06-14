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
public class ThemeController: ControllerBase
{
    private readonly ILogger<ThemeController> _logger;
    private readonly IThemeRepository _repository;
    private readonly IMapper _mapper;

    public ThemeController(IThemeRepository repository, IMapper mapper, ILogger<ThemeController> logger)
    {
        _repository = repository
            ?? throw new ArgumentNullException(nameof(repository));
        _mapper = mapper
            ?? throw new ArgumentNullException(nameof(mapper));
        _logger = logger
            ?? throw new ArgumentNullException(nameof(logger));
    }

    [HttpGet("{plannerId:guid}/{themeId:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ThemeDto>> RetrieveThemeAsync(Guid plannerId, Guid themeId, CancellationToken cancellationToken)
    {
        try 
        {
            ThemeEntity? themeEntity = await _repository.RetrieveAsync(themeId, plannerId, cancellationToken);
            if (themeEntity is null) 
            {
                _logger.LogInformation($"Unable to retrive theme with theme id: {themeId}");
                return NotFound();
            }
            ThemeDto theme = _mapper.Map<ThemeDto>(themeEntity);
            return Ok(theme);
        } catch (Exception ex)
        {
            _logger.LogError($"Unable to retrieve theme with theme id: {themeId}, planner id: {plannerId}", ex);
            return StatusCode(500, "A problem happened while trying to process your request");
        }
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<ThemeDto>>> ListThemesAsync(CancellationToken cancellationToken)
    {
        try 
        {
            IEnumerable<ThemeEntity> themeEntities = await _repository.ListAsync(cancellationToken);
            IEnumerable<ThemeDto> themes = _mapper.Map<IEnumerable<ThemeDto>>(themeEntities);
            return Ok(themes);
        } catch (Exception ex)
        {
            _logger.LogError(ex, "Unable to list themes");
            return StatusCode(500, "A problem happened while trying to process your request");
        }
    }

    [HttpGet("{plannerId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<ThemeDto>>> ListThemesForPlannerAsync(Guid plannerId, CancellationToken cancellationToken)
    {
        try 
        {
            IEnumerable<ThemeEntity> themeEntities = await _repository.ListPlannerThemesAsync(plannerId, cancellationToken);
            IEnumerable<ThemeDto> themes = _mapper.Map<IEnumerable<ThemeDto>>(themeEntities);
            return Ok(themes);
        } catch (Exception ex)
        {
            _logger.LogError($"Unable to list themes for planner id: {plannerId}", ex);
            return StatusCode(500, "A problem happened while trying to process your request");
        }
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateThemeAsync([FromBody] ThemeDto theme, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        try 
        {
            ThemeEntity themeEntity = _mapper.Map<ThemeEntity>(theme);
            await _repository.CreateAsync(themeEntity, cancellationToken);
            await _repository.SaveChangesAsync(cancellationToken);
            ThemeDto returnedTheme = _mapper.Map<ThemeDto>(themeEntity);
            return CreatedAtAction(nameof(RetrieveThemeAsync), new { id = themeEntity.themeId}, returnedTheme);
        } catch (Exception ex)
        {
            _logger.LogError($"Unable to create theme for theme, {theme}", ex);
            return StatusCode(500, "A problem happened while trying to process your request");
        }
    }

    [HttpDelete("{plannerId:guid}/{themeId:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteThemeAsync(Guid plannerId, Guid themeId, CancellationToken cancellationToken)
    {
        try 
        {
            ThemeEntity? themeEntity = await _repository.RetrieveAsync(plannerId, themeId, cancellationToken);
            if(themeEntity is null) 
            {
                _logger.LogInformation($"Unable to retrive theme with theme id: {themeId}");
                return NotFound();
            }
            await _repository.DeleteAsync(themeEntity, cancellationToken);
            await _repository.SaveChangesAsync(cancellationToken);
            return NoContent();
        } catch (Exception ex)
        {
            _logger.LogError($"Unable to delete theme with theme id {themeId}, planner id {plannerId}", ex);
            return StatusCode(500, "A problem happened while trying to process your request");
        }
    }

    [HttpPut("{plannerId:guid}/{themeId:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> UpdateThemeAsync(Guid plannerId, Guid themeId, [FromBody] ThemeDto theme, CancellationToken cancellationToken)
    {
        try 
        {
            if ( plannerId != theme.plannerId && themeId != theme.themeId) return BadRequest ("ID in URL doesn't match the body"); 
            ThemeEntity? themeEntity = await _repository.RetrieveAsync(themeId, plannerId, cancellationToken);
            if (themeEntity is null) 
            { 
                _logger.LogInformation($"Unable to retrive theme with theme id: {themeId}");
                return NotFound();
            }
            _mapper.Map(theme, themeEntity);
            await _repository.UpdateAsync(themeEntity, cancellationToken);
            await _repository.SaveChangesAsync(cancellationToken);
            return NoContent();
        } catch (Exception ex)
        {
            _logger.LogError($"Unable to update theme with theme id {themeId}, planner id {plannerId}", ex);
            return StatusCode(500, "A problem happened while trying to process your request");
        }
    }
}
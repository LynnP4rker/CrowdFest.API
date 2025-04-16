using AutoMapper;
using CrowdFest.API.Abstractions.Repositories;
using CrowdFest.API.Entities;
using CrowdFest.API.Models;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("id/[controller]")]
public class ThemeController: ControllerBase
{
    private readonly IThemeRepository _repository;
    private readonly IMapper _mapper;

    public ThemeController(IThemeRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    [HttpGet("{plannerId:guid}/{themeId:guid}")]
    public async Task<ActionResult<ThemeDto>> RetrieveThemeAsync(Guid plannerId, Guid themeId, CancellationToken cancellationToken)
    {
        ThemeEntity? themeEntity = await _repository.RetrieveAsync(themeId, plannerId, cancellationToken);
        if (themeEntity is null) return NotFound();
        ThemeDto theme = _mapper.Map<ThemeDto>(themeEntity);
        return Ok(theme);
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<ThemeDto>>> ListThemesAsync(CancellationToken cancellationToken)
    {
        IEnumerable<ThemeEntity> themeEntities = await _repository.ListAsync(cancellationToken);
        IEnumerable<ThemeDto> themes = _mapper.Map<IEnumerable<ThemeDto>>(themeEntities);
        return Ok(themes);
    }

    [HttpGet("{plannerId}")]
    public async Task<ActionResult<IEnumerable<ThemeDto>>> ListThemesForPlannerAsync(Guid plannerId, CancellationToken cancellationToken)
    {
        IEnumerable<ThemeEntity> themeEntities = await _repository.ListPlannerThemesAsync(plannerId, cancellationToken);
        IEnumerable<ThemeDto> themes = _mapper.Map<IEnumerable<ThemeDto>>(themeEntities);
        return Ok(themes);
    }

    [HttpPost]
    public async Task<IActionResult> CreateThemeAsync([FromBody] ThemeDto theme, CancellationToken cancellationToken)
    {
        ThemeEntity themeEntity = _mapper.Map<ThemeEntity>(theme);
        await _repository.CreateAsync(themeEntity, cancellationToken);
        await _repository.SaveChangesAsync(cancellationToken);
        ThemeDto returnedTheme = _mapper.Map<ThemeDto>(themeEntity);
        return CreatedAtAction(nameof(RetrieveThemeAsync), new { id = themeEntity.themeId}, returnedTheme);
    }

    [HttpDelete("{plannerId:guid}/{themeId:guid}")]
    public async Task<IActionResult> DeleteThemeAsync(Guid plannerId, Guid themeId, CancellationToken cancellationToken)
    {
        ThemeEntity? themeEntity = await _repository.RetrieveAsync(plannerId, themeId, cancellationToken);
        if(themeEntity is null) return NotFound();
        await _repository.DeleteAsync(themeEntity, cancellationToken);
        await _repository.SaveChangesAsync(cancellationToken);
        return NoContent();
    }

    [HttpPut("{plannerId:guid}/{themeId:guid}")]
    public async Task<IActionResult> UpdateThemeAsync(Guid plannerId, Guid themeId, [FromBody] ThemeDto theme, CancellationToken cancellationToken)
    {
        if ( plannerId != theme.plannerId && themeId != theme.themeId) return BadRequest ("ID in URL doesn't match the body"); 
        ThemeEntity? themeEntity = await _repository.RetrieveAsync(themeId, plannerId, cancellationToken);
        if (themeEntity is null) return NotFound();
        _mapper.Map(theme, themeEntity);
        await _repository.UpdateAsync(themeEntity, cancellationToken);
        await _repository.SaveChangesAsync(cancellationToken);
        return NoContent();
    }
}
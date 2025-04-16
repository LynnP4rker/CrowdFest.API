using AutoMapper;
using CrowdFest.API.Abstractions.Repositories;
using CrowdFest.API.Entities;
using CrowdFest.API.Models;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("id/[controller]")]
public class UserController: ControllerBase
{
    private readonly IUserRepository _repository;
    private readonly IMapper _mapper;

    public UserController(IUserRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<UserDto>> RetrieveUserAsync(Guid id, CancellationToken cancellationToken)
    {
        UserEntity? userEntity = await _repository.RetrieveAsync(id, cancellationToken);
        if (userEntity is null) return NotFound();
        UserDto user = _mapper.Map<UserDto>(userEntity);
        return Ok(user);
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<UserDto>>> ListUsersAsync(CancellationToken cancellationToken)
    {
        IEnumerable<UserEntity> userEntities = await _repository.ListAsync(cancellationToken);
        IEnumerable<UserDto> users = _mapper.Map<IEnumerable<UserDto>>(userEntities);
        return Ok(users);
    }

    [HttpPost]
    public async Task<IActionResult> CreateUserAsync([FromBody] UserDto user, CancellationToken cancellationToken)
    {
        UserEntity userEntity = _mapper.Map<UserEntity>(user);
        await _repository.CreateAsync(userEntity, cancellationToken);
        await _repository.SaveChangesAsync(cancellationToken);
        UserDto returnedUser = _mapper.Map<UserDto>(userEntity);
        return CreatedAtAction(nameof(RetrieveUserAsync), new { id = userEntity.id}, returnedUser);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteUserAsync(Guid id, CancellationToken cancellationToken)
    {
        UserEntity? userEntity = await _repository.RetrieveAsync(id, cancellationToken);
        if (userEntity is null) return NoContent();
        await _repository.DeleteAsync(userEntity, cancellationToken);
        await _repository.SaveChangesAsync(cancellationToken);
        return NoContent();
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateUserAsync(Guid id, [FromBody] UserDto user, CancellationToken cancellationToken)
    {
        UserEntity? userEntity = await _repository.RetrieveAsync(id, cancellationToken);
        if (userEntity is null) return NotFound();
        _mapper.Map(user, userEntity);
        await _repository.UpdateAsync(userEntity, cancellationToken);
        await _repository.SaveChangesAsync(cancellationToken);
        return NoContent();
    }
}
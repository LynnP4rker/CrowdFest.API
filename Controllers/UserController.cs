using AutoMapper;
using CrowdFest.API.Abstractions.Repositories;
using CrowdFest.API.Entities;
using CrowdFest.API.Models;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("id/[controller]")]
[ProducesResponseType(StatusCodes.Status401Unauthorized)]
public class UserController: ControllerBase
{
    private readonly IUserRepository _repository;
    private readonly IMapper _mapper;

    public UserController(IUserRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    [HttpGet("{emailaddress}")]
    public async Task<ActionResult<UserDto>> RetrieveUserAsync(string emailaddress, CancellationToken cancellationToken)
    {
        UserEntity? userEntity = await _repository.RetrieveAsync(emailaddress, cancellationToken);
        if (userEntity is null) return NotFound();
        UserDto user = _mapper.Map<UserDto>(userEntity);
        return Ok(user);
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

    [HttpDelete("{emailaddress}")]
    public async Task<IActionResult> DeleteUserAsync(string emailaddress, CancellationToken cancellationToken)
    {
        UserEntity? userEntity = await _repository.RetrieveAsync(emailaddress, cancellationToken);
        if (userEntity is null) return NoContent();
        await _repository.DeleteAsync(userEntity, cancellationToken);
        await _repository.SaveChangesAsync(cancellationToken);
        return NoContent();
    }

    [HttpPut("{emailaddress}")]
    public async Task<IActionResult> UpdateUserAsync(string emailaddress, [FromBody] UserDto user, CancellationToken cancellationToken)
    {
        UserEntity? userEntity = await _repository.RetrieveAsync(emailaddress, cancellationToken);
        if (userEntity is null) return NotFound();
        _mapper.Map(user, userEntity);
        await _repository.UpdateAsync(userEntity, cancellationToken);
        await _repository.SaveChangesAsync(cancellationToken);
        return NoContent();
    }
}
using AutoMapper;
using CrowdFest.API.Abstractions.Repositories;
using CrowdFest.API.Entities;
using CrowdFest.API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("id/[controller]")]
[Authorize]
[ProducesResponseType(StatusCodes.Status401Unauthorized)]
public class UserController: ControllerBase
{
    private readonly IUserRepository _repository;
    private readonly IMapper _mapper;
    private readonly IPasswordService _password;

    public UserController(IUserRepository repository, IMapper mapper, IPasswordService password)
    {
        _repository = repository;
        _mapper = mapper;
        _password = password;
    }

    [HttpGet("{emailaddress}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<UserDto>> RetrieveUserAsync(string emailaddress, CancellationToken cancellationToken)
    {
        UserEntity? userEntity = await _repository.RetrieveAsync(emailaddress, cancellationToken);
        if (userEntity is null) return NotFound();
        UserDto user = _mapper.Map<UserDto>(userEntity);
        return Ok(user);
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public async Task<IActionResult> CreateUserAsync([FromBody] RegisterDto user, CancellationToken cancellationToken)
    {
        UserEntity userEntity = _mapper.Map<UserEntity>(user);
        userEntity.passwordHash = _password.Hash(user.password);
        await _repository.CreateAsync(userEntity, cancellationToken);
        await _repository.SaveChangesAsync(cancellationToken);
        UserDto returnedUser = _mapper.Map<UserDto>(userEntity);
        return CreatedAtAction(nameof(RetrieveUserAsync), new { id = userEntity.id}, returnedUser);
    }

    [HttpDelete("{emailaddress}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteUserAsync(string emailaddress, CancellationToken cancellationToken)
    {
        UserEntity? userEntity = await _repository.RetrieveAsync(emailaddress, cancellationToken);
        if (userEntity is null) return NoContent();
        await _repository.DeleteAsync(userEntity, cancellationToken);
        await _repository.SaveChangesAsync(cancellationToken);
        return NoContent();
    }

    [HttpPut("{emailaddress}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
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
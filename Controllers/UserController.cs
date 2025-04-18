using AutoMapper;
using CrowdFest.API.Abstractions.Repositories;
using CrowdFest.API.Entities;
using CrowdFest.API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("id/[controller]")]
[ProducesResponseType(StatusCodes.Status500InternalServerError)]
public class UserController: ControllerBase
{
    private readonly ILogger<UserController> _logger; 
    private readonly IUserRepository _repository;
    private readonly IMapper _mapper;
    private readonly IPasswordService _password;

    public UserController(IUserRepository repository, IMapper mapper, IPasswordService password, ILogger<UserController> logger)
    {
        _repository = repository
            ?? throw new ArgumentNullException(nameof(repository));
        _mapper = mapper
            ?? throw new ArgumentNullException(nameof(mapper));
        _logger = logger
            ?? throw new ArgumentNullException(nameof(logger));
        _password = password
            ?? throw new ArgumentNullException(nameof(password));
    }

    [HttpGet("{emailaddress}")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<UserDto>> RetrieveUserAsync(string emailaddress, CancellationToken cancellationToken)
    {
        try 
        {
            UserEntity? userEntity = await _repository.RetrieveAsync(emailaddress, cancellationToken);
            if (userEntity is null) 
            {   
                _logger.LogInformation($"Unable to retrieve email address: {emailaddress}");
                return NotFound();
            }
            UserDto user = _mapper.Map<UserDto>(userEntity);
            return Ok(user);
        } catch (Exception ex)
        {
            _logger.LogError($"Unable to retrieve email address: {emailaddress}", ex);
            return StatusCode(500, "A problem happened while trying to process your request");
        }
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateUserAsync([FromBody] RegisterDto user, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        try 
        {
            UserEntity userEntity = _mapper.Map<UserEntity>(user);
            userEntity.passwordHash = _password.Hash(user.password);
            await _repository.CreateAsync(userEntity, cancellationToken);
            await _repository.SaveChangesAsync(cancellationToken);
            UserDto returnedUser = _mapper.Map<UserDto>(userEntity);
            return CreatedAtAction(nameof(RetrieveUserAsync), new { id = userEntity.id}, returnedUser);
        } catch (Exception ex)
        {
            _logger.LogError($"Unable to create user {user}", ex);
            return StatusCode(500, "A problem happened while trying to process your request");
        }
    }

    [HttpDelete("{emailaddress}")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteUserAsync(string emailaddress, CancellationToken cancellationToken)
    {
        try 
        {
            UserEntity? userEntity = await _repository.RetrieveAsync(emailaddress, cancellationToken);
            if (userEntity is null) 
            {   
                _logger.LogInformation($"Unable to retrieve email address: {emailaddress}");
                return NoContent();
            }
            await _repository.DeleteAsync(userEntity, cancellationToken);
            await _repository.SaveChangesAsync(cancellationToken);
            return NoContent();
        } catch (Exception ex)
        {
            _logger.LogError($"Unable to delete user with email address: {emailaddress}", ex);
            return StatusCode(500, "A problem happened while trying to process your request");
        }
    }

    [HttpPut("{emailaddress}")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> UpdateUserAsync(string emailaddress, [FromBody] UserDto user, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        try 
        {
            UserEntity? userEntity = await _repository.RetrieveAsync(emailaddress, cancellationToken);
            if (userEntity is null) 
            {
                _logger.LogInformation($"Unable to retrieve email address: {emailaddress}");
                return NotFound();
            }
            _mapper.Map(user, userEntity);
            await _repository.UpdateAsync(userEntity, cancellationToken);
            await _repository.SaveChangesAsync(cancellationToken);
            return NoContent();
        } catch(Exception ex)
        {
            _logger.LogInformation($"Unable to update user with email address: {emailaddress}", ex);
            return StatusCode(500, "A problem happened while trying to process your request");
        }
    }
}
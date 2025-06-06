using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using AutoMapper;
using CrowdFest.API.Abstractions.Repositories;
using CrowdFest.API.Entities;
using CrowdFest.API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace CrowdFest.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[ProducesResponseType(StatusCodes.Status500InternalServerError)]
public class PlannerController: ControllerBase
{
    private readonly IConfiguration _configuration;
    private readonly ILogger<PlannerController> _logger;
    private readonly IPlannerRepository _repository;
    private readonly IPlannerAccountRepository _account;
    private readonly IPasswordService _password;
    private readonly IMapper _mapper;
    public PlannerController(IPlannerRepository repository, 
        IPlannerAccountRepository account, 
        IMapper mapper, 
        ILogger<PlannerController> logger, 
        IPasswordService password,
        IConfiguration configuration)
    {
        _repository = repository
            ?? throw new ArgumentNullException(nameof(repository));
        _account = account
            ?? throw new ArgumentNullException(nameof(account));
        _mapper = mapper
            ?? throw new ArgumentNullException(nameof(mapper));
        _logger = logger
            ?? throw new ArgumentNullException(nameof(logger));
        _password = password
            ?? throw new ArgumentNullException(nameof(logger));
        _configuration = configuration
            ?? throw new ArgumentNullException(nameof(configuration));
    }

    [HttpGet("{id}")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<PlannerDto>> RetrievePlannerAsync(Guid id, CancellationToken cancellationToken)
    {
        try 
        {
            PlannerEntity? plannerEntity = await _repository.RetrieveAsync(id, cancellationToken);
            if(plannerEntity is null) 
            {
                _logger.LogInformation($"Unable to retrieve planner id: {id}");
                return NotFound();
            }
            PlannerDto planner = _mapper.Map<PlannerDto>(plannerEntity);
            return Ok(planner);
        } catch (Exception ex)
        {
            _logger.LogError($"Unable to retrieve planner with planner id: {id}", ex);
            return StatusCode(500, "A problem happened while trying to process your request");
        }
    }

    [HttpGet("{emailaddress}")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<PlannerDto>> RetrievePlannerFromEmailAddressAsync(string emailaddress, CancellationToken cancellationToken)
    {
        try 
        {
            PlannerEntity? plannerEntity = await _repository.RetrieveEmailAsync(emailaddress, cancellationToken);
            if (plannerEntity is null)
            {
                _logger.LogInformation($"Unable to retrieve planner emailaddress: {emailaddress}");
                return NotFound();
            }
            PlannerDto planner = _mapper.Map<PlannerDto>(plannerEntity);
            return Ok(planner);
        } catch (Exception ex)
        {
            _logger.LogError($"Unable to retrieve planner with planner id: {emailaddress}", ex);
            return StatusCode(500, "A problem happened while trying to process your request");
        }
    }

    [HttpGet]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<IEnumerable<PlannerDto>>> ListPlannersAsync(CancellationToken cancellationToken)
    {
        try 
        {
            IEnumerable<PlannerEntity> plannerEntities = await _repository.ListAsync(cancellationToken);
            IEnumerable<PlannerDto> planners = _mapper.Map<IEnumerable<PlannerDto>>(plannerEntities);
            return Ok(planners);
        } catch (Exception ex)
        {
            _logger.LogError(ex, "Unable to list planners");
            return StatusCode(500, "A problem happened while trying to process your request");
        }
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreatePlannerAsync([FromBody] RegisterDto planner, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid) 
        { 
            return BadRequest(ModelState); 
        }
        try 
        {
            //Create planner
            PlannerEntity plannerEntity = _mapper.Map<PlannerEntity>(planner);
            plannerEntity.passwordHash = _password.Hash(planner.password);

            await _repository.CreateAsync(plannerEntity, cancellationToken);

            string otp = RandomNumberGenerator.GetInt32(100000, 999999).ToString("D6");

            PlannerAccountEntity plannerAccountEntity = new PlannerAccountEntity
            {
                id = plannerEntity.id,
                Otp = otp,
                GeneratedAt = DateTime.UtcNow,
                ExpiresAt = DateTime.UtcNow.AddMinutes(15)
            };

            await _account.CreateAsync(plannerAccountEntity, cancellationToken);
            await _repository.SaveChangesAsync(cancellationToken);

            PlannerDto plannerDto = _mapper.Map<PlannerDto>(plannerEntity);
            return CreatedAtAction(nameof(RetrievePlannerAsync), new { id = plannerEntity.id}, plannerDto);
        } catch (Exception ex)
        {
            _logger.LogError($"Unable to create planner with email: {planner.emailAddress}", ex);
            return StatusCode(500, "A problem happened while trying to process your request");
        }
    }

    [HttpPost("{id}/otp")]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GeneratePlannerOtp(Guid id, CancellationToken cancellationToken)
    {
        try
        {   
            PlannerEntity? plannerEntity = await _repository.RetrieveAsync(id, cancellationToken);
            if (plannerEntity is null) 
            {
                _logger.LogInformation($"Unable to find planner with id: {id}");
                return NotFound();
            }
            
            PlannerAccountEntity? accountEntity = await _account.RetrieveAsync(id, cancellationToken);
            if (accountEntity is null)
            {
                _logger.LogInformation($"Unable to find account with id: {id}");
                return NotFound(); 
            }
            
            string otp = RandomNumberGenerator.GetInt32(100000, 999999).ToString("D6");

            accountEntity.Otp = otp;
            accountEntity.GeneratedAt = DateTime.UtcNow;
            accountEntity.ExpiresAt = DateTime.UtcNow.AddMinutes(15);

            await _repository.SaveChangesAsync(cancellationToken);

            return Ok("Otp successfully generated");
        } catch (Exception ex)
        {
            _logger.LogError($"{ex}");
            return StatusCode(500, "A problem happened while trying to process your request");
        }
    } 

    [HttpPost("verify")]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> VerifyOtp([FromBody] VerifyDto request, CancellationToken cancellationToken)
    {
        PlannerAccountEntity? accountEntity = await _account.RetrieveAsync(request.id, cancellationToken);
        if (accountEntity is null
            || accountEntity.Otp != request.otp
            || accountEntity.ExpiresAt < DateTime.UtcNow)
        {
            return Unauthorized("Invalid or Expired otp");
        }

        PlannerEntity? plannerEntity = await _repository.RetrieveAsync(request.id, cancellationToken);
        if (plannerEntity is null)
        {
            _logger.LogInformation($"Unable to retrieve planner id: {request.id}");
            return NotFound();
        }

        accountEntity.Otp = null;
        accountEntity.GeneratedAt = null;
        accountEntity.ExpiresAt = null;

        await _account.UpdateAsync(accountEntity, cancellationToken);
        await _account.SaveChangesAsync(cancellationToken);

        var securityKey = new SymmetricSecurityKey(
            Convert.FromBase64String(_configuration["Authentication:SecretForKey"]));
        
        var signingCredentials = new SigningCredentials(
            securityKey, SecurityAlgorithms.HmacSha256);
        
        var claimsForToken = new List<Claim>();
        claimsForToken.Add(new Claim("sub", plannerEntity.id.ToString()));
        claimsForToken.Add(new Claim("first_name", plannerEntity.firstName));
        claimsForToken.Add(new Claim("last_name", plannerEntity.lastName));

        var jwtSecurityToken = new JwtSecurityToken(
            _configuration["Authentication:Issuer"],
            _configuration["Authentication:Audience"],
            claimsForToken,
            DateTime.UtcNow,
            DateTime.UtcNow.AddMinutes(15),
            signingCredentials
        );

        var tokenToReturn = new JwtSecurityTokenHandler()
            .WriteToken(jwtSecurityToken);
        
        return Ok(tokenToReturn);
    }

    [HttpDelete("{id}")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> RemovePlannerAsync(Guid id, CancellationToken cancellationToken)
    {
        try {

            PlannerEntity? plannerEntity = await _repository.RetrieveAsync(id, cancellationToken);
            if (plannerEntity is null) 
            {
                _logger.LogInformation($"Unable to retrieve planner id: {id}");
                return NotFound();
            }
            await _repository.DeleteAsync(plannerEntity, cancellationToken);
            await _repository.SaveChangesAsync(cancellationToken);
            return NoContent();

        } catch (Exception ex)
        {
            _logger.LogError($"Unable to remove planner with planner id: {id}", ex);
            return StatusCode(500, "A problem happened while trying to process your request");
        }
    }

    [HttpPut("{id}")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> UpdatePlannerAsync(Guid id, [FromBody] PlannerDto planner, CancellationToken cancellationToken)
    {
        try 
        {
            if (id != planner.id) { return BadRequest ("ID in URL doesn't match the body"); }
            PlannerEntity? plannerEntity = await _repository.RetrieveAsync(id, cancellationToken);
            if (plannerEntity is null) { return NotFound(); }
            _mapper.Map(planner, plannerEntity);
            await _repository.UpdateAsync(plannerEntity, cancellationToken);
            await _repository.SaveChangesAsync(cancellationToken);
            return NoContent();
        } catch (Exception ex)
        {
            _logger.LogError($"Unable to update planner with planner id: {id}", ex);
            return StatusCode(500, "A problem happened while trying to process your request");
        }
    }
}
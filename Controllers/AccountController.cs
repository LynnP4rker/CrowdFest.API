using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using AutoMapper;
using CrowdFest.API.Abstractions.Repositories;
using CrowdFest.API.Entities;
using CrowdFest.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

[ApiController]
[Route("api/[controller]")]
public class AccountController: ControllerBase
{
    private readonly IMapper _mapper;
    private readonly IPasswordService _password;
    private readonly IPlannerRepository _repository;
    private readonly IConfiguration _configuration;
    private readonly IPlannerAccountRepository _account;
    private readonly ILogger<PlannerEntity> _logger;

    public AccountController (
        IMapper mapper,
        IPasswordService password,
        IPlannerRepository repository,
        IConfiguration configuration,
        IPlannerAccountRepository account,
        ILogger<PlannerEntity> logger
    )
    {
        _mapper = mapper;
        _password = password;
        _repository = repository;
        _configuration = configuration;
        _account = account;
        _logger = logger;
    }

    [HttpPost("Planner")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreatePlannerAccountAsync([FromBody] RegisterDto planner, CancellationToken cancellationToken)
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
            return Ok();
        } catch (Exception ex)
        {
            _logger.LogError($"Unable to create planner with email: {planner.emailAddress}", ex);
            return StatusCode(500, "A problem happened while trying to process your request");
        }
    }

    [HttpPost("Planner/{id}/otp")]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GeneratePlannerAccountOtp(Guid id, CancellationToken cancellationToken)
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

    [HttpPost("Planner/otp/verify")]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> VerifyPlannerAccountOtp([FromBody] VerifyDto request, CancellationToken cancellationToken)
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
}
using System.Security.Cryptography;
using AutoMapper;
using CrowdFest.API.Abstractions.Repositories;
using CrowdFest.API.Entities;
using CrowdFest.API.Models;
using Microsoft.AspNetCore.Mvc;
[ApiController]
[Route("api/[controller]")]
public class AccountController: ControllerBase
{
    private readonly IMapper _mapper;
    private readonly IPasswordService _password;
    private readonly IPlannerRepository _plannerRepository;
    private readonly IOrganizationRepository _organisationRepository;
    private readonly IConfiguration _configuration;
    private readonly IPlannerAccountRepository _plannerAccount;
    private readonly IOrganizationAccountRepository _organisationAccount;
    private readonly ILogger<PlannerEntity> _logger;

    public AccountController (
        IMapper mapper,
        IPasswordService password,
        IPlannerRepository plannerRepository,
        IOrganizationRepository organisationRepository,
        IConfiguration configuration,
        IPlannerAccountRepository plannerAccount,
        IOrganizationAccountRepository organisationAccount,
        ILogger<PlannerEntity> logger
    )
    {
        _mapper = mapper;
        _password = password;
        _plannerRepository = plannerRepository;
        _organisationRepository = organisationRepository;
        _configuration = configuration;
        _plannerAccount = plannerAccount;
        _organisationAccount = organisationAccount;
        _logger = logger;
    }

    [HttpPost("Planner")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreatePlannerAccountAsync([FromBody] RegisterPlannerDto planner, CancellationToken cancellationToken)
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

            await _plannerRepository.CreateAsync(plannerEntity, cancellationToken);

            string otp = RandomNumberGenerator.GetInt32(100000, 999999).ToString("D6");

            PlannerAccountEntity plannerAccountEntity = new PlannerAccountEntity
            {
                id = plannerEntity.id,
                Otp = otp,
                GeneratedAt = DateTime.UtcNow,
                ExpiresAt = DateTime.UtcNow.AddMinutes(15)
            };

            await _plannerAccount.CreateAsync(plannerAccountEntity, cancellationToken);
            await _plannerRepository.SaveChangesAsync(cancellationToken);

            return Ok($"{plannerEntity.id}");
        } catch (Exception ex)
        {
            _logger.LogError($"Unable to create planner with email: {planner.emailAddress}", ex);
            return StatusCode(500, "A problem happened while trying to process your request");
        }
    }

    [HttpPost("Organization")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateOrganizationAccountAsync([FromBody] RegisterOrganizationDto organisation, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid) 
        { 
            return BadRequest(ModelState); 
        }
        try 
        {
            //Create Organisation
            OrganizationEntity organisationEntity = _mapper.Map<OrganizationEntity>(organisation);
            organisationEntity.passwordHash = _password.Hash(organisation.password);
            await _organisationRepository.CreateAsync(organisationEntity, cancellationToken);

            string Otp = RandomNumberGenerator.GetInt32(100000, 999999).ToString("D6");
            
            OrganizationAccountEntity organisationAccountEntity = new OrganizationAccountEntity
            {
                id = organisationEntity.id,
                otp = Otp,
                GeneratedAt = DateTime.UtcNow,
                ExpiresAt = DateTime.UtcNow.AddMinutes(15)
            };

            await _organisationAccount.CreateAsync(organisationAccountEntity, cancellationToken);
            await _organisationRepository.SaveChangesAsync(cancellationToken);

            return Ok();
        } catch (Exception ex)
        {
            _logger.LogError($"Unable to create organisation with email: {organisation.emailAddress}", ex);
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
            PlannerEntity? plannerEntity = await _plannerRepository.RetrieveAsync(id, cancellationToken);
            if (plannerEntity is null) 
            {
                _logger.LogInformation($"Unable to find planner with id: {id}");
                return NotFound();
            }
            
            PlannerAccountEntity? accountEntity = await _plannerAccount.RetrieveAsync(id, cancellationToken);
            if (accountEntity is null)
            {
                _logger.LogInformation($"Unable to find account with id: {id}");
                return NotFound(); 
            }
            
            string otp = RandomNumberGenerator.GetInt32(100000, 999999).ToString("D6");

            accountEntity.Otp = otp;
            accountEntity.GeneratedAt = DateTime.UtcNow;
            accountEntity.ExpiresAt = DateTime.UtcNow.AddMinutes(15);

            await _plannerAccount.UpdateAsync(accountEntity, cancellationToken);
            await _plannerRepository.SaveChangesAsync(cancellationToken);

            return Ok("Otp successfully generated");
        } catch (Exception ex)
        {
            _logger.LogError($"{ex}");
            return StatusCode(500, "A problem happened while trying to process your request");
        }
    } 

    [HttpPost("Organization/{id}/otp")]
    public async Task<IActionResult> GenerateOrganizationOtp(Guid id, CancellationToken cancellationToken)
    {
        try
        {
            OrganizationEntity? organizationEntity = await _organisationRepository.RetrieveAsync(id, cancellationToken);
            if (organizationEntity is null)
            {
                _logger.LogInformation($"Unable to find organization with id: {id}");
                return NotFound();
            }

            OrganizationAccountEntity? organizationAccountEntity = await _organisationAccount.RetrieveAsync(id, cancellationToken);
            if (organizationAccountEntity is null)
            {
                _logger.LogInformation($"Unable to find account with id: {id}");
                return NotFound();
            }

            string Otp = RandomNumberGenerator.GetInt32(100000, 999999).ToString("D6");
            organizationAccountEntity.otp = Otp;
            organizationAccountEntity.GeneratedAt = DateTime.UtcNow;
            organizationAccountEntity.ExpiresAt = DateTime.UtcNow.AddMinutes(15);

            await _organisationAccount.UpdateAsync(organizationAccountEntity, cancellationToken);
            await _organisationRepository.SaveChangesAsync(cancellationToken);

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
        PlannerAccountEntity? accountEntity = await _plannerAccount.RetrieveAsync(request.id, cancellationToken);
        if (accountEntity is null
            || accountEntity.Otp != request.otp
            || accountEntity.ExpiresAt < DateTime.UtcNow)
        {
            return Unauthorized("Invalid or Expired otp");
        }

        PlannerEntity? plannerEntity = await _plannerRepository.RetrieveAsync(request.id, cancellationToken);
        if (plannerEntity is null)
        {
            _logger.LogInformation($"Unable to retrieve planner id: {request.id}");
            return NotFound();
        }

        accountEntity.Otp = null;
        accountEntity.isVerified = true;

        await _plannerAccount.UpdateAsync(accountEntity, cancellationToken);
        await _plannerAccount.SaveChangesAsync(cancellationToken);

        return Ok("Account verified");
    }

    [HttpPost("Organization/otp/verify")]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> VerifyOrganisationAccountOtp([FromBody] VerifyDto request, CancellationToken cancellationToken)
    {
        OrganizationAccountEntity? organizationAccountEntity = await _organisationAccount.RetrieveAsync(request.id, cancellationToken);
        if (organizationAccountEntity is null
            || organizationAccountEntity.otp != request.otp
            || organizationAccountEntity.ExpiresAt < DateTime.UtcNow)
        {
            return Unauthorized("Invalid or Expired otp");
        }

        OrganizationEntity? organizationEntity = await _organisationRepository.RetrieveAsync(request.id, cancellationToken);
        if (organizationEntity is null)
        {
            _logger.LogInformation($"Unable to retrieve organisation id: {request.id}");
            return NotFound();
        }

        organizationAccountEntity.otp = null;
        organizationAccountEntity.isVerified = true;

        await _organisationAccount.UpdateAsync(organizationAccountEntity, cancellationToken);
        await _organisationAccount.SaveChangesAsync(cancellationToken);
        
        return Ok("Account verified");
    }
}
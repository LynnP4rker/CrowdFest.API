using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using CrowdFest.API.Abstractions.Repositories;
using CrowdFest.API.Entities;
using CrowdFest.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

[ApiController]
[Route("api/[controller]")]
[ProducesResponseType(StatusCodes.Status401Unauthorized)]
public class AuthenticationController: ControllerBase
{
    private readonly IConfiguration _configuration;
    private readonly IPlannerRepository _plannerRepository;
    private readonly IOrganizationRepository _organizationRepository;
    private readonly IPlannerAccountRepository _plannerAccount;
    private readonly IOrganizationAccountRepository _organizationAccount;
    private readonly IPasswordService _password;

    public AuthenticationController(IConfiguration configuration, 
        IPlannerRepository plannerRepository, 
        IPasswordService password, 
        IPlannerAccountRepository plannerAccount,
        IOrganizationRepository organizationRepository,
        IOrganizationAccountRepository organizationAccount
    )
    {
        _configuration = configuration
            ?? throw new ArgumentNullException(nameof(configuration));
        _plannerRepository = plannerRepository
            ?? throw new ArgumentNullException(nameof(plannerRepository));
        _password = password
            ?? throw new ArgumentNullException(nameof(password));
        _plannerAccount = plannerAccount
            ?? throw new ArgumentNullException(nameof(plannerAccount));
        _organizationRepository = organizationRepository
            ?? throw new ArgumentNullException(nameof(organizationRepository));
        _organizationAccount = organizationAccount
            ?? throw new ArgumentNullException(nameof(organizationAccount));
    }

    [HttpPost("/Planner")]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<string>> Authenticate(
        [FromBody] LoginDto user, CancellationToken cancellationToken
    )
    {
        //Step 1: Retrieve the user
        PlannerEntity? plannerEntity = await _plannerRepository.RetrieveEmailAsync(user.emailAddress, cancellationToken);
        if(plannerEntity is null) return Unauthorized("Invalid details");

        //Step 2: Verify the password
        bool isPasswordValid = _password.Verify(plannerEntity.passwordHash, user.password);
        if (!isPasswordValid) return Unauthorized("Invalid details");

        //Step 3: Verify account status
        PlannerAccountEntity? plannerAccountEntity = await _plannerAccount.RetrieveAsync(plannerEntity.id, cancellationToken);
        if (plannerAccountEntity is null
            || plannerAccountEntity.isVerified == false
        )
        return Unauthorized("Invalid access");

        //Step 4: Generate otp
        var securityKey = new SymmetricSecurityKey(
            Convert.FromBase64String(_configuration["Authentication:SecretForKey"]));
        
        var signingCredentials = new SigningCredentials(
            securityKey, SecurityAlgorithms.HmacSha256);
        
        var claimsForToken = new List<Claim>();
        claimsForToken.Add(new Claim("sub", plannerEntity.id.ToString()));
        claimsForToken.Add(new Claim("display_name", plannerEntity.displayName));
        claimsForToken.Add(new Claim("email", plannerEntity.emailAddress));

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

    [HttpPost("/Organization")]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<string>> AuthenticateOrganization(
        [FromBody] LoginDto user, CancellationToken cancellationToken
    )
    {
        //Step 1: Retrieve the user 
        OrganizationEntity? organizationEntity = await _organizationRepository.RetrieveEmailAsync(user.emailAddress, cancellationToken);
        if(organizationEntity is null) return Unauthorized("Invalid details");

        //Step 2: Verify the password
        bool isPasswordValid = _password.Verify(organizationEntity.passwordHash, user.password);
        if (!isPasswordValid) return Unauthorized("Invalid details");

        //Step 3: Verify account status
        OrganizationAccountEntity? organizationAccountEntity = await _organizationAccount.RetrieveAsync(organizationEntity.id, cancellationToken);
        if (organizationAccountEntity is null
            || organizationAccountEntity.isVerified == false
        )
        return Unauthorized("Invalid access");

        //Step 4: Generate otp
        var securityKey = new SymmetricSecurityKey(
            Convert.FromBase64String(_configuration["Authentication:SecretForKey"]));
        
        var signingCredentials = new SigningCredentials(
            securityKey, SecurityAlgorithms.HmacSha256);
        
        var claimsForToken = new List<Claim>();
        claimsForToken.Add(new Claim("sub", organizationEntity.id.ToString()));
        claimsForToken.Add(new Claim("org_name", organizationEntity.name));

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
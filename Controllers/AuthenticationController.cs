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
    private readonly IPlannerRepository _repository;
    private readonly IPlannerAccountRepository _account;
    private readonly IPasswordService _password;

    public AuthenticationController(IConfiguration configuration, IPlannerRepository repository, IPasswordService password, IPlannerAccountRepository account)
    {
        _configuration = configuration
            ?? throw new ArgumentNullException(nameof(configuration));
        _repository = repository
            ?? throw new ArgumentNullException(nameof(repository));
        _password = password
            ?? throw new ArgumentNullException(nameof(password));
        _account = account
            ?? throw new ArgumentNullException(nameof(account));
    }

    [HttpPost]
    public async Task<ActionResult<string>> Authenticate(
        [FromBody] LoginDto user, CancellationToken cancellationToken
    )
    {
        //Step 1: Retrieve the user
        PlannerEntity? plannerEntity = await _repository.RetrieveEmailAsync(user.emailAddress, cancellationToken);
        if(plannerEntity is null) return Unauthorized("Invalid details");

        //Step 2: Verify the password
        bool isPasswordValid = _password.Verify(plannerEntity.passwordHash, user.password);
        if (!isPasswordValid) return Unauthorized("Invalid details");

        //Generate Otp
        string otp = RandomNumberGenerator.GetInt32(100000, 999999).ToString("D6");

        PlannerAccountEntity? accountEntity = await _account.RetrieveAsync(plannerEntity.id, cancellationToken);
        if (accountEntity is null)
        {
            return Unauthorized("Invalid details");
        }

        accountEntity.id = plannerEntity.id;
        accountEntity.Otp = otp;
        accountEntity.GeneratedAt = DateTime.UtcNow;
        accountEntity.ExpiresAt = DateTime.UtcNow.AddMinutes(15);

        await _account.UpdateAsync(accountEntity, cancellationToken);
        await _account.SaveChangesAsync(cancellationToken);

        return Ok("OTP sent. Please verify to login");
    }
}
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Threading.Tasks;
using CrowdFest.API.Abstractions.Repositories;
using CrowdFest.API.Entities;
using CrowdFest.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

[ApiController]
[Route("api/[controller]")]
public class AuthenticationController: ControllerBase
{
    private readonly IConfiguration _configuration;
    private readonly IUserRepository _repository;
    private readonly IPasswordService _password;

    public AuthenticationController(IConfiguration configuration, IUserRepository repository, IPasswordService password)
    {
        _configuration = configuration;
        _repository = repository;
        _password = password;
    }

    [HttpPost]
    public async Task<ActionResult<string>> Authenticate(
        [FromBody] LoginDto user, CancellationToken cancellationToken
    )
    {
        //Step 1: Retrieve the user
        UserEntity? userEntity = await _repository.RetrieveAsync(user.emailAddress, cancellationToken);
        if(userEntity is null) return Unauthorized("Invalid details");

        //Step 2: Verify the password
        bool isPasswordValid = _password.Verify(userEntity.passwordHash, user.password);
        if (!isPasswordValid) return Unauthorized("Invalid details");

        //Step 3: Create a token
        var securityKey = new SymmetricSecurityKey(
            Convert.FromBase64String(_configuration["Authentication:SecretForKey"]));
        
        var signingCredentials = new SigningCredentials(
            securityKey, SecurityAlgorithms.HmacSha256);
        
        var claimsForToken = new List<Claim>();
        claimsForToken.Add(new Claim("sub", userEntity.id.ToString()));
        claimsForToken.Add(new Claim("first_name", userEntity.lastName));
        claimsForToken.Add(new Claim("last_name", userEntity.lastName));

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
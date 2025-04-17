namespace CrowdFest.API.Models;
public record LoginDto(
    string emailAddress,
    string password
);
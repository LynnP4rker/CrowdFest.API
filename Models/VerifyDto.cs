namespace CrowdFest.API.Models;

public record VerifyDto(
    Guid id,
    string otp
);
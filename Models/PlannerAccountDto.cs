namespace CrowdFest.API.Models;

public record PlannerAccountDto(
    Guid id,
    int otp,
    DateTime GeneratedAt,
    DateTime ExpiresAt
);
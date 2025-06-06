namespace CrowdFest.API.Entities;

public class PlannerAccountEntity
{
    public Guid id { get; set; }
    public string? Otp { get; set; }
    public DateTime? GeneratedAt { get; set; }
    public DateTime? ExpiresAt { get; set; }
}
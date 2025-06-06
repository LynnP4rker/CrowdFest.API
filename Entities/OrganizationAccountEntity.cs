namespace CrowdFest.API.Entities;

public class OrganizationAccountEntity
{
    public Guid id { get; set; }
    public string? otp { get; set; }
    public DateTime? GeneratedAt { get; set; }
    public DateTime? ExpiresAt { get; set; }
    public bool isVerified { get; set; }

}
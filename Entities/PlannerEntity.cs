using CrowdFest.API.Enums;

namespace CrowdFest.API.Entities;

public class PlannerEntity 
{
    public Guid id { get; set; }
    public string displayName { get; set; }
    public string firstName { get; set; }
    public string lastName { get; set; }
    public Gender gender { get; set; }
    public DateOnly dob { get; set; }
    public string emailAddress { get; set; }
    public string phoneNumber { get; set; }

    public PlannerEntity()
    {
        displayName = String.Empty;
        firstName = String.Empty;
        lastName = String.Empty;
        emailAddress = String.Empty;
        phoneNumber = String.Empty;
    }
}
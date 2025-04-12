using CrowdFest.API.Enums;

namespace CrowdFest.API.Models;

public record PlannerDto (
    Guid id,
    string displayName,
    string firstName,
    string lastName,
    Gender gender,
    DateOnly dob,
    string emailAddress,
    string phoneNumber
);
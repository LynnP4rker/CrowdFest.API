using CrowdFest.API.Enums;
public record RegisterPlannerDto(
    string displayName,
    string firstName,
    string lastName,
    Gender gender,
    DateOnly dob,
    string emailAddress,
    string phoneNumber,
    string password
);
namespace CrowdFest.API.Models;

public record LocationDto (
    Guid locationId,
    Guid plannerId,
    string name,
    string description
);
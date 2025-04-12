namespace CrowdFest.API.Models;

public record PostDto (
    Guid id,
    Guid plannerId,
    string description,
    int rating
);
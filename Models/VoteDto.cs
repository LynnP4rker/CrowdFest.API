using CrowdFest.API.Enums;

namespace CrowdFest.API.Models;

public record VoteDto (
    Guid id,
    Guid groupId,
    Guid eventId,
    string title,
    string description,
    DateTime date,
    Guid locationId,
    Guid themeId,
    Priority priority
);
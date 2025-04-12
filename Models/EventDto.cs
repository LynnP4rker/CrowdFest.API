using CrowdFest.API.Enums;

namespace CrowdFest.API.Models;
public record EventDto (
    Guid id,
    Guid groupId,
    int attendees,
    int absentees,
    string title,
    string description,
    DateTime date,
    string location,
    string theme,
    Priority priority
);
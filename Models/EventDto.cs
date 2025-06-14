using CrowdFest.API.Enums;

namespace CrowdFest.API.Models;
public record EventDto (
    Guid id,
    Guid GroupId,
    string Title,
    string Description,
    Category Category,
    Guid ThemeId,
    DateOnly Date,
    TimeSpan Time,
    Guid LocationId,
    Priority Priority,
    int Attendees,
    int Absentees
);
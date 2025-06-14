namespace CrowdFest.API.Models;

public record ThemeDto (
    Guid themeId,
    Guid plannerId,
    string name
);
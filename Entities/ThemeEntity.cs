namespace CrowdFest.API.Entities;

public class ThemeEntity
{
    public Guid themeId { get; set; }
    public Guid plannerId { get; set; }
    public string name { get; set; }
    public string  nameNormalised { get; set; }
    public string description { get; set; }

    public ThemeEntity()
    {
        name = String.Empty;
        nameNormalised = String.Empty;
        description = String.Empty;
    }
}
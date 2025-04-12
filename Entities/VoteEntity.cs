using CrowdFest.API.Enums;

namespace CrowdFest.API.Entities;

public class VoteEntity 
{
    public Guid id { get; set; }
    public Guid groupId { get; set; }
    public Guid eventId { get; set; }
    public string title { get; set; }
    public string description { get; set; }
    public DateTime date { get; set; }
    public Guid locationId { get; set; }
    public Guid themeId { get; set; }
    public Priority priority { get; set; }

    public VoteEntity()
    {
        title = String.Empty;
        description = String.Empty;
    }
}
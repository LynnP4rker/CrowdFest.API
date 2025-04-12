using CrowdFest.API.Enums;

namespace CrowdFest.API.Entities;

public class EventEntity 
{
    public Guid id { get; set; }
    public Guid groupId { get; set; }
    public int attendees { get; set; }
    public int absentees { get; set; }
    public string title { get; set; }
    public string description { get; set; }
    public DateTime date { get; set; }
    public string location { get; set; }
    public string theme { get; set; }
    public Priority priority { get; set; }

    public EventEntity()
    {
        title = String.Empty;
        description = String.Empty;
        location = String.Empty;
        theme = String.Empty;
    }
}
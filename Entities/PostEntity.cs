namespace CrowdFest.API.Entities;

public class PostEntity 
{
    public Guid id { get; set; }
    public Guid plannerId { get; set; }
    public string description { get; set; }
    public int rating { get; set; }

    public PostEntity()
    {
        description = String.Empty;
    }
}
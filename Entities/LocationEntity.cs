namespace CrowdFest.API.Entities;

public class LocationEntity 
{
    public Guid locationId { get; set; }
    public Guid plannerId { get; set; }
    public string name { get; set; }
    public string nameNormalised { get; set; }
    public string description { get; set; }

    public LocationEntity()
    {
        name = String.Empty;
        nameNormalised = String.Empty;
        description = String.Empty;
    }
}
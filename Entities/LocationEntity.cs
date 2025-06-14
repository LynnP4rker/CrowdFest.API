using CrowdFest.API.Enums;

namespace CrowdFest.API.Entities;

public class LocationEntity 
{
    public Guid locationId { get; set; }
    public Guid plannerId { get; set; }
    public string address1 { get; set; }
    public string? address2 { get; set; }
    public string city { get; set; }
    public County county { get; set; }
    public string postCode { get; set; }

    public LocationEntity()
    {
        address1 = String.Empty;
        city = String.Empty;
        postCode = String.Empty;
    }
}
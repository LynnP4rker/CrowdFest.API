using CrowdFest.API.Enums;

namespace CrowdFest.API.Models;

public record LocationDto (
    Guid locationId,
    Guid plannerId,
    string address1,
    string address2,
    string city,
    County county,
    string postCode
);
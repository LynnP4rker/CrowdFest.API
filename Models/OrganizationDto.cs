namespace CrowdFest.API.Models;

public record OrganizationDto
(
    Guid id, 
    string name, 
    string emailAddress,
    string telephoneNumber,
    string Address1,
    string Address2,
    string City,
    string PostCode,
    string Country
);
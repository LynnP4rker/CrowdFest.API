namespace CrowdFest.API.Models;

public record RegisterOrganizationDto
(
    string name, 
    string emailAddress,
    string password,
    string telephoneNumber,
    string Address1,
    string Address2,
    string City,
    string PostCode,
    string Country
);
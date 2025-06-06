namespace CrowdFest.API.Entities;

public class OrganizationEntity
{
    public Guid id { get; set; }
    public string name { get; set; }
    public string nameNormalised { get; set; }
    public string emailAddress { get; set; }
    public string passwordHash { get; set; }
    public string telephoneNumber { get; set; }
    public string Address1 { get; set; }
    public string Address2 { get; set; }
    public string City { get; set; }
    public string PostCode { get; set; }
    public string Country { get; set; }

    public OrganizationEntity()
    {
        name = String.Empty;
        nameNormalised = String.Empty;
        emailAddress = String.Empty;
        passwordHash = String.Empty;
        telephoneNumber = String.Empty;
        Address1 = String.Empty;
        Address2 = String.Empty;
        City = String.Empty;
        PostCode = String.Empty;
        Country = String.Empty;
    }
}
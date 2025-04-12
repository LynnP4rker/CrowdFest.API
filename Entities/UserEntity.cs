namespace CrowdFest.API.Entities;

public class UserEntity 
{
    public Guid id { get; set; }
    public string emailAddress { get; set; }
    public string passwordHash { get; set; }
    public string firstName { get; set; }
    public string lastName { get; set; }

    public UserEntity ()
    {
        emailAddress = String.Empty;
        passwordHash = String.Empty;
        firstName = String.Empty;
        lastName = String.Empty;
    }
}
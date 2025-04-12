namespace CrowdFest.API.Entities;

public class UserEntity 
{
    public string emailAddress { get; set; }
    public string password { get; set; }

    public UserEntity ()
    {
        emailAddress = String.Empty;
        password = String.Empty;
    }
}
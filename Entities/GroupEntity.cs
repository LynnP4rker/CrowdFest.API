using CrowdFest.API.Enums;
namespace CrowdFest.API.Entities;

public class GroupEntity
{
    public Guid id { get; set; }
    private string _name = String.Empty;
    private string? _description = String.Empty;
    private string? _accessCode = String.Empty;
    public string Name
    {
        get => _name;
        set
        {
            _name = value;
            NameNormalised = new string(value
                .Where(c => !char.IsWhiteSpace(c))
                .ToArray())
                .ToUpperInvariant();
        }
    }
    public string? Description
    {
        get => _description;
        set => _description = value;
    }
    public string? AccessCode
    {
        get => _accessCode;
        set => _accessCode = value;
    }

    public string NameNormalised { get; private set; } = String.Empty;
    
    public Guid GroupLeaderId { get; set; }
    public Status Status { get; set; }

    public bool IsPrivate { get; set; }
    public bool IsOrganisationGroup { get; set; }
    public bool RequiresApproval { get; set; }


}
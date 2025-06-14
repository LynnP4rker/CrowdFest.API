using CrowdFest.API.Enums;

namespace CrowdFest.API.Models;

public record GroupDto (
    Guid id,
    string name,
    Guid groupLeaderId,
    Status status,
    bool isPrivate,
    bool isOrganisationGroup,
    bool requiresApproval,
    string description
);
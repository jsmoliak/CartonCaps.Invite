using Microsoft.AspNetCore.Authorization;

namespace CartonCaps.Invite.API.Authorization
/// <summary>
/// Represents an authorization requirement for ownership of a resource.
/// </summary>
{
    public class OwnershipRequirement : IAuthorizationRequirement { }
}

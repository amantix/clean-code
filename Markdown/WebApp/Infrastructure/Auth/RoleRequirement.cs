using Microsoft.AspNetCore.Authorization;

namespace Infrastructure.Auth;

public class RoleRequirement(params string[] roles) : IAuthorizationRequirement
{
    public string[] Roles { get; } = roles;
}

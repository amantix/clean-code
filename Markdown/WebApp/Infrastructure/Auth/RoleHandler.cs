using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace Infrastructure.Auth;

public class RoleHandler : AuthorizationHandler<RoleRequirement>
{
    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, RoleRequirement requirement)
    {
        var roleClaim = context.User.FindAll(ClaimTypes.Role).Select(claim => claim.Value);

        if (requirement.Roles.Any(role => roleClaim.Contains(role)))
        {
            context.Succeed(requirement);
        }

        return Task.CompletedTask;
    }
}

using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;

namespace Infrastructure.Auth;

public class RolePolicyProvider(IOptions<AuthorizationOptions> options) : DefaultAuthorizationPolicyProvider(options)
{
    private readonly AuthorizationOptions _options = options.Value;

    public override Task<AuthorizationPolicy?> GetPolicyAsync(string policyName)
    {
        if (policyName.StartsWith("RolePolicy:", StringComparison.OrdinalIgnoreCase))
        {
            var roles = policyName.Substring("RolePolicy:".Length).Split(',');

            var policy = new AuthorizationPolicyBuilder()
                .AddRequirements(new RoleRequirement(roles))
                .Build();

            return Task.FromResult(policy)!;
        }

        return base.GetPolicyAsync(policyName);
    }
}
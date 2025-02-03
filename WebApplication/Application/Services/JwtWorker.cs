using System.Security.Claims;
using System.Text;
using Domain.Models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using Application.Abstract.Services;
using Microsoft.Extensions.Options;

namespace Application.Services;

public class JwtWorker(IOptions<JwtOptions> options): IJwtWorker
{
    public string? GenerateJwtToken(User user)
    {
        try
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(options.Value.Secret));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new Claim[]
            {
                new(options.Value.UserId, user.Id.ToString()),
                new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var token = new JwtSecurityToken(
                issuer: options.Value.Issuer,
                audience: options.Value.Audience,
                claims: claims,
                expires: DateTime.Now.AddMinutes(options.Value.ExpireMinutes),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
        catch (Exception ex)
        {
            return null;
        }
    }

    public (bool isSuccess, Guid userId) ValidateToken(string token)
    {
        var tokenHandler = new JwtSecurityTokenHandler();

        var validationsParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateIssuerSigningKey = true,
            ValidateLifetime = true,
            ValidIssuer = options.Value.Issuer,
            ValidAudience = options.Value.Audience,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(options.Value.Secret))
        };
        try
        {
            var claimsPrincipal = tokenHandler.ValidateToken(token, validationsParameters, out var validatedToken);
            var userId = Guid.Parse(claimsPrincipal.Claims.First(x => x.Type == options.Value.UserId).Value);
            return (true, userId);
        }
        catch (Exception ex)
        {
            return (false, default);
        }
        
    }

}
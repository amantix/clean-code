using Data.Entities;
using Microsoft.Extensions.Options;
using Services.Abstracts;
using Services.Schemas;
using System.Security.Claims;
using System.Text;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;


namespace Services.Services
{
    public class JWTService(IOptions<JWTSetting> options) : IJWTService
    {
        public string GenerateToken(User user)
        {
            var claims = new List<Claim>
        {
            new Claim("email", user.Email!),
            new Claim("username", user.Username!),
            new Claim("Id", user.Id.ToString())
            };

            var jwtToken = new JwtSecurityToken(
                expires: DateTime.UtcNow.Add(options.Value.Lifetime),
                claims: claims,
                signingCredentials: new SigningCredentials(
                    new SymmetricSecurityKey(Encoding.UTF8.GetBytes(options.Value.Key!)),
                    SecurityAlgorithms.HmacSha256)
            );

            return new JwtSecurityTokenHandler().WriteToken(jwtToken);
        }

    }
}

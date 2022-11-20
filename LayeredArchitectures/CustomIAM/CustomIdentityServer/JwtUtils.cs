using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using CustomIdentityServer.Models;
using Microsoft.IdentityModel.Tokens;

namespace CustomIdentityServer;

public class JwtUtils : IJwtUtils
{
    private readonly string _issuer;

    public JwtUtils(string issuer)
    {
        _issuer = issuer ?? throw new ArgumentNullException(nameof(issuer));
    }

    public string GenerateToken(User user, string clientSecret)
    {
        var key = Encoding.UTF8.GetBytes(clientSecret);
        var tokenHandler = new JwtSecurityTokenHandler();

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[]
            {
                new Claim("id", user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Name, user.Name),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
            }),
            Claims = new Dictionary<string, object>()
            {
                {"roles", user.Roles.Select(x=>x.Title)},  
                {"permissions", user.Roles.SelectMany(x=>x.Permissions).Select(x=>x.Title).Distinct()}  
            },
            Expires = DateTime.UtcNow.AddMinutes(1),
            Issuer = _issuer,
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key),
           SecurityAlgorithms.HmacSha512Signature)
        };

        var token = tokenHandler.CreateJwtSecurityToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }
}

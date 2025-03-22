using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

public static class JwtManager
{
    public static string GenerateToken(string userId, string role, string secretKey, int expireMinutes = 20)
    {
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var now = DateTime.UtcNow;
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Role, role),
            new Claim("UserId", userId)
        };

        var token = new JwtSecurityToken(
            issuer: "blog-manager-db",
            audience: "myapp-users",
            claims: claims,
            notBefore: now,
            expires: now.AddMinutes(expireMinutes),
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}

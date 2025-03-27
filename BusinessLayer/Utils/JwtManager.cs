using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

public static class JwtManager
{
    public static string GenerateToken(string userId, string userName, string role, string secretKey, int expireMinutes = 20)
    {
        SymmetricSecurityKey key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
        SigningCredentials creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        DateTime now = DateTime.UtcNow;
        List<Claim> claims = new List<Claim>
        {
            new Claim(ClaimTypes.Role, role),
            new Claim(ClaimTypes.Name, userName),
            new Claim("UserId", userId),
        };

        JwtSecurityToken token = new JwtSecurityToken(
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

using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using TwitterCloneBack.Model.User.Model;

namespace TwitterCloneBack.Login;

public class JwtTokenGenerator(
    IConfiguration configuration)
{
    public string GenerateJwt(UserDto user)
    {
        var claims =
            new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email)
            };

        var key =
            new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(configuration["Jwt:Key"]!)
            );

        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token =
            new JwtSecurityToken(
                configuration["Jwt:Issuer"],
                configuration["Jwt:Audience"],
                claims,
                expires: DateTime.UtcNow.AddHours(2),
                signingCredentials: creds
            );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
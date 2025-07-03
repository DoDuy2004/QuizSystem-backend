using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using QuizSystem_backend.Enums;
using QuizSystem_backend.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
namespace QuizSystem_backend.services;

public class TokenService
{
    private readonly IConfiguration _config;
    private readonly UserManager<AppUser> _userManager;

    public TokenService(IConfiguration config,UserManager<AppUser> userManager)
    {
        _config = config;
        _userManager = userManager;
    }
    public async Task<string> CreateTokenAsync(AppUser user)
    {
        var userRoles = await _userManager.GetRolesAsync(user);
        // Tạo claims cho token
        var authClaims = new List<Claim>
    {
        new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
        new Claim(ClaimTypes.Email, user.Email!),
        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
    };

        foreach (var role in userRoles)
        {
            authClaims.Add(new Claim(ClaimTypes.Role, role));
        }
        var extraClaims = await _userManager.GetClaimsAsync(user);
        foreach (var claim in extraClaims)
        {
            authClaims.Add(new Claim(claim.Type, claim.Value));
        }

        var jwtSettings = _config.GetSection("Jwt");
        var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["Key"]!));

        var token = new JwtSecurityToken(
            issuer: jwtSettings["Issuer"],
            audience: jwtSettings["Audience"],
            expires: DateTime.UtcNow.AddHours(6),
            claims: authClaims,
            signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}

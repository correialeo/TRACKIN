using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;

namespace Trackin.Tests.Integration;

public class TestTokenValidator : ISecurityTokenValidator
{
    public bool CanValidateToken => true;

    public int MaximumTokenSizeInBytes { get; set; } = int.MaxValue;

    public bool CanReadToken(string securityToken) => true;

    public ClaimsPrincipal ValidateToken(string securityToken, TokenValidationParameters validationParameters, out SecurityToken validatedToken)
    {
        validatedToken = new JwtSecurityToken();

        var claims = new[]
        {
            new Claim(ClaimTypes.Name, "Test user"),
            new Claim(ClaimTypes.NameIdentifier, "1"),
            new Claim(ClaimTypes.Email, "test@example.com"),
            new Claim("Id", "1"),
            new Claim(ClaimTypes.Role, "ADMINISTRADOR")
        };

        return new ClaimsPrincipal(new ClaimsIdentity(claims, "Bearer"));
    }
}
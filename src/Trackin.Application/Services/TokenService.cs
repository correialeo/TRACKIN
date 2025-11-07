
using Trackin.Application.Interfaces;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using Trackin.Domain.Entity;
using Microsoft.Extensions.Configuration;

namespace Trackin.Application.Services
{
    public class TokenService : ITokenService
    {
        private readonly string _secretKey;
        private readonly IConfiguration _configuration;
        private readonly string _issuer;
        private readonly string _audience;

        public TokenService(IConfiguration configuration)
        {
            _configuration = configuration;
            _secretKey = _configuration["Jwt:SecretKey"];
            _issuer = _configuration["Jwt:Issuer"] ?? "SCED.API";
            _audience = _configuration["Jwt:Audience"];
        }

        public string GenerateToken(Usuario user)
        {
            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
            byte[] key = Encoding.ASCII.GetBytes(_secretKey);

            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Nome),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, user.Role.ToString()),
                new Claim("UserId", user.Id.ToString()),
                new Claim("PatioId", user.PatioId.ToString()),
            };

            SecurityTokenDescriptor tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddHours(24),
                Issuer = _issuer,
                Audience = _audience,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            SecurityToken token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }


        public ClaimsPrincipal? ValidateToken(string token)
        {
            try
            {
                JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
                byte[] key = Encoding.ASCII.GetBytes(_secretKey);

                TokenValidationParameters validationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = true,
                    ValidIssuer = _issuer,
                    ValidateAudience = true,
                    ValidAudience = _audience,
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero
                };

                ClaimsPrincipal principal = tokenHandler.ValidateToken(token, validationParameters, out SecurityToken validatedToken);
                return principal;
            }
            catch (Exception)
            {
                return null;
            }
        }
        
        public string? GetUsernameFromToken(string token)
        {
            ClaimsPrincipal? principal = ValidateToken(token);
            return principal?.FindFirst(ClaimTypes.Name)?.Value;
        }

        public long? GetUserIdFromToken(string token)
        {
            ClaimsPrincipal? principal = ValidateToken(token);
            string? userIdClaim = principal?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            return long.TryParse(userIdClaim, out long userId) ? userId : null;
        }
    }
}
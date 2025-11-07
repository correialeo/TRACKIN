using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Trackin.Domain.Entity;

namespace Trackin.Application.Interfaces
{
    public interface ITokenService
    {
        string GenerateToken(Usuario user);
        ClaimsPrincipal? ValidateToken(string token);
        string? GetUsernameFromToken(string token);
        long? GetUserIdFromToken(string token);
    }
}
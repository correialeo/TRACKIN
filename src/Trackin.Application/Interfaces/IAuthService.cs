using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Trackin.Application.DTOs;

namespace Trackin.Application.Interfaces
{
    public interface IAuthService
    {
        Task<LoginResponse> LoginAsync(LoginRequest request);
        Task<UserDTO> RegisterAsync(RegisterRequest request);
        Task<bool> ValidateTokenAsync(string token);
        Task<UserDTO?> GetUserFromTokenAsync(string token);
    }
}
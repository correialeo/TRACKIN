
using System.ComponentModel;
using Trackin.Application.DTOs;
using Trackin.Application.Interfaces;
using Trackin.Domain.Entity;
using Trackin.Domain.Interfaces;

namespace Trackin.Application.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUsuarioRepository _userRepository;
        private readonly ITokenService _tokenService;

        public AuthService(IUsuarioRepository userRepository, ITokenService tokenService)
        {
            _userRepository = userRepository;
            _tokenService = tokenService;
        }

        public async Task<LoginResponse> LoginAsync(LoginRequest request)
        {
            try
            {
                Usuario? user = await _userRepository.GetByEmailAsync(request.Email);

                if (user == null || !BCrypt.Net.BCrypt.Verify(request.Password, user.SenhaHash))
                {
                    throw new UnauthorizedAccessException("Credenciais inválidas");
                }

                string token = _tokenService.GenerateToken(user);

                return new LoginResponse
                {
                    Token = token,
                    Username = user.Nome,
                    Role = user.Role,
                    ExpiresAt = DateTime.UtcNow.AddHours(24)
                };
            }
            catch (Exception ex)
            {
                throw new UnauthorizedAccessException("Erro ao autenticar usuário", ex);
            }
        }

        public async Task<UserDTO> RegisterAsync(RegisterRequest request)
        {
            if (await _userRepository.ExistsAsync(request.Username))
            {
                throw new InvalidOperationException("Username já existe");
            }

            string passwordHash = BCrypt.Net.BCrypt.HashPassword(request.Password);

            Usuario user = new Usuario(
                request.Username,
                request.Email,
                passwordHash,
                request.Role,
                request.PatioId
            );

            Usuario createdUser = await _userRepository.CreateAsync(user);

            return new UserDTO
            {
                Id = createdUser.Id,
                Username = createdUser.Nome,
                Email = createdUser.Email,
                Role = createdUser.Role,
                PatioId = createdUser.PatioId
            };
        }

        public async Task<bool> ValidateTokenAsync(string token)
        {
            return _tokenService.ValidateToken(token) != null;
        }

        public async Task<UserDTO?> GetUserFromTokenAsync(string token)
        {
            long? userId = _tokenService.GetUserIdFromToken(token);
            if (userId == null) return null;

            Usuario? user = await _userRepository.GetByIdAsync(userId.Value);
            if (user == null) return null;

            return new UserDTO
            {
                Id = user.Id,
                Username = user.Nome,
                Email = user.Email,
                Role = user.Role,
                PatioId = user.PatioId
            };
        }
    }
}
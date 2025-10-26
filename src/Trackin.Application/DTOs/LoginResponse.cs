
using Trackin.Domain.Enums;

namespace Trackin.Application.DTOs
{
    public class LoginResponse
    {
        public string Token { get; set; }
        public string Username { get; set; }
        public UsuarioRole Role { get; set; }
        public DateTime ExpiresAt { get; set; }
    }
}
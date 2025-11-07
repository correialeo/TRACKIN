
using Trackin.Domain.Enums;

namespace Trackin.Application.DTOs
{
    public class RegisterRequest
    {
        public string Username { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public UsuarioRole Role { get; set; }
        public long PatioId { get; set; }
    }
}
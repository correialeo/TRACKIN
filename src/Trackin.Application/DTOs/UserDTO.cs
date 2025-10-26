
using Trackin.Domain.Enums;

namespace Trackin.Application.DTOs
{
    public class UserDTO
    {
        public long Id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public UsuarioRole Role { get; set; }
        public long? PatioId { get; set; }
    }
}
using Trackin.Domain.Enums;

namespace Trackin.Domain.Entity
{
    public class Usuario
    {
        public long Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Senha { get; set; } = string.Empty;
        public UsuarioRole Role { get; set; } = UsuarioRole.COMUM; // Admin, Operador, Gerente
        public long PatioId { get; set; } 
        public EventoMoto[]? Eventos { get; set; } = Array.Empty<EventoMoto>();


        public Patio Patio { get; set; } 
    }
}

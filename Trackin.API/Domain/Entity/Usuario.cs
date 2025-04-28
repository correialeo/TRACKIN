using Trackin.API.Domain.Enums;

namespace Trackin.API.Domain.Entity
{
    public class Usuario
    {
        public long Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Senha { get; set; } = string.Empty;
        public UsuarioRole Role { get; set; } = UsuarioRole.COMUM; // Admin, Operador, Gerente
        public long FilialId { get; set; } // FK para Filial
    }
}

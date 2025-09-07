using System.Security.Cryptography;
using System.Text;
using Trackin.Domain.Enums;

namespace Trackin.Domain.Entity
{
    public class Usuario
    {
        public long Id { get; private set; }
        public string Nome { get; private set; } = string.Empty;
        public string Email { get; private set; } = string.Empty;
        public string SenhaHash { get; private set; } = string.Empty;
        public UsuarioRole Role { get; private set; } = UsuarioRole.COMUM;
        public long PatioId { get; private set; }
        public bool Ativo { get; private set; } = true;
        public DateTime DataCriacao { get; private set; }
        public DateTime? UltimoLogin { get; private set; }

        public Patio Patio { get; private set; }
        private readonly List<EventoMoto> _eventos = new();
        public IReadOnlyCollection<EventoMoto> Eventos => _eventos.AsReadOnly();

        protected Usuario() { }

        public Usuario(string nome, string email, string senha, UsuarioRole role, long patioId)
        {
            ValidarParametrosUsuario(nome, email, senha);

            Nome = nome;
            Email = email.ToLowerInvariant();
            SenhaHash = GerarHashSenha(senha);
            Role = role;
            PatioId = patioId;
            Ativo = true;
            DataCriacao = DateTime.UtcNow;
        }

        public bool ValidarSenha(string senha)
        {
            if (string.IsNullOrWhiteSpace(senha))
                return false;

            return VerificarHashSenha(senha, SenhaHash);
        }

        public void AlterarSenha(string senhaAtual, string novaSenha)
        {
            if (!ValidarSenha(senhaAtual))
                throw new UnauthorizedAccessException("Senha atual incorreta");

            ValidarForcaSenha(novaSenha);
            SenhaHash = GerarHashSenha(novaSenha);
        }

        public void RegistrarLogin()
        {
            if (!Ativo)
                throw new InvalidOperationException("Usuário inativo não pode fazer login");

            UltimoLogin = DateTime.UtcNow;
        }

        public void DesativarUsuario()
        {
            if (!Ativo)
                throw new InvalidOperationException("Usuário já está inativo");

            Ativo = false;
        }

        public void AtivarUsuario()
        {
            if (Ativo)
                throw new InvalidOperationException("Usuário já está ativo");

            Ativo = true;
        }

        public void AlterarRole(UsuarioRole novaRole)
        {
            if (Role == novaRole)
                throw new InvalidOperationException("Usuário já possui esta role");

            Role = novaRole;
        }

        public bool PodeGerenciarUsuarios()
        {
            return Role == UsuarioRole.ADMINISTRADOR || Role == UsuarioRole.GERENTE;
        }

        public bool PodeAcessarRelatorios()
        {
            return Role != UsuarioRole.COMUM;
        }

        public bool PodeCriarEventosManualmente()
        {
            return Role == UsuarioRole.ADMINISTRADOR || Role == UsuarioRole.COMUM || Role == UsuarioRole.GERENTE;
        }

        public void AdicionarEvento(EventoMoto evento)
        {
            if (evento == null)
                throw new ArgumentNullException(nameof(evento));

            if (evento.UsuarioId != Id)
                throw new ArgumentException("Evento não pertence a este usuário");

            if (!PodeCriarEventosManualmente())
                throw new UnauthorizedAccessException("Usuário não tem permissão para criar eventos");

            _eventos.Add(evento);
        }

        public TimeSpan TempoSemLogin()
        {
            if (!UltimoLogin.HasValue)
                return DateTime.UtcNow - DataCriacao;

            return DateTime.UtcNow - UltimoLogin.Value;
        }

        public bool EstaInativoHaMuitoTempo(TimeSpan tempoLimite)
        {
            return TempoSemLogin() > tempoLimite;
        }

        public IEnumerable<EventoMoto> ObterEventosRecentes(TimeSpan periodo)
        {
            var dataLimite = DateTime.UtcNow - periodo;
            return _eventos.Where(e => e.Timestamp >= dataLimite);
        }

        private string GerarHashSenha(string senha)
        {
            using var sha256 = SHA256.Create();
            var salt = Guid.NewGuid().ToString();
            var senhaComSalt = senha + salt;
            var hashBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(senhaComSalt));
            var hash = Convert.ToBase64String(hashBytes);
            return $"{salt}:{hash}";
        }

        private bool VerificarHashSenha(string senha, string senhaHash)
        {
            var partes = senhaHash.Split(':');
            if (partes.Length != 2) return false;

            var salt = partes[0];
            var hash = partes[1];

            using var sha256 = SHA256.Create();
            var senhaComSalt = senha + salt;
            var hashBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(senhaComSalt));
            var hashCalculado = Convert.ToBase64String(hashBytes);

            return hash == hashCalculado;
        }

        private void ValidarParametrosUsuario(string nome, string email, string senha)
        {
            if (string.IsNullOrWhiteSpace(nome))
                throw new ArgumentException("Nome não pode ser vazio", nameof(nome));

            if (string.IsNullOrWhiteSpace(email))
                throw new ArgumentException("Email não pode ser vazio", nameof(email));

            if (!email.Contains("@"))
                throw new ArgumentException("Email deve ter formato válido", nameof(email));

            ValidarForcaSenha(senha);
        }

        private void ValidarForcaSenha(string senha)
        {
            if (string.IsNullOrWhiteSpace(senha))
                throw new ArgumentException("Senha não pode ser vazia");

            if (senha.Length < 6)
                throw new ArgumentException("Senha deve ter pelo menos 6 caracteres");

            if (!senha.Any(char.IsUpper))
                throw new ArgumentException("Senha deve conter ao menos uma letra maiúscula");

            if (!senha.Any(char.IsLower))
                throw new ArgumentException("Senha deve conter ao menos uma letra minúscula");

            if (!senha.Any(char.IsDigit))
                throw new ArgumentException("Senha deve conter ao menos um número");
        }
    }
}

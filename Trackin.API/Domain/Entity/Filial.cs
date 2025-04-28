namespace Trackin.API.Domain.Entity
{
    public class Filial
    {
        public long Id { get; set; } 
        public string Nome { get; set; } = string.Empty; 
        public long PatioId { get; set; } // FK para Pátio
        public string Telefone { get; set; } = string.Empty; 
        public string Email { get; set; } = string.Empty; 

        public ICollection<Usuario> Usuarios { get; set; } = new List<Usuario>(); 
        public Patio Patio { get; set; }
    }
}

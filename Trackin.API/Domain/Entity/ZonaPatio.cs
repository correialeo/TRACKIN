namespace Trackin.API.Domain.Entity
{
    public class ZonaPatio
    {
        public long Id { get; set; }
        public long PatioId { get; set; } // FK para Pátio
        public string Nome { get; set; } = string.Empty; // Manutenção, Estacionamento, Lavagem, etc
        public double CoordenadaInicialX { get; set; } 
        public double CoordenadaInicialY { get; set; } 
        public double CoordenadaFinalX { get; set; } 
        public double CoordenadaFinalY { get; set; } 
        public string Cor { get; set; } = string.Empty; // Cor para representação visual
    }
}

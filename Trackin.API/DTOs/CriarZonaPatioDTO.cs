using Trackin.API.Domain.Enums;

namespace Trackin.API.DTOs
{
    public class CriarZonaPatioDTO
    {
        public long PatioId { get; set; }
        public string Nome { get; set; } = string.Empty; // Manutenção, Estacionamento, Lavagem, etc
        public TipoZona TipoZona { get; set; }
        public double CoordenadaInicialX { get; set; }
        public double CoordenadaInicialY { get; set; }
        public double CoordenadaFinalX { get; set; }
        public double CoordenadaFinalY { get; set; }
        public string Cor { get; set; } = string.Empty; // Cor para representação visual    
    }
}


using Trackin.Domain.Enums;

namespace Trackin.Application.DTOs
{
    public class LocalizacaoMotoDTO
    {
        public long Id { get; set; } 
        public long MotoId { get; set; } 
        public long PatioId { get; set; } 
        public double CoordenadaX { get; set; } 
        public double CoordenadaY { get; set; } 
        public DateTime Timestamp { get; set; } 
        public MotoStatus Status { get; set; } 
        public FonteDados FonteDados { get; set; } 
        public double Confiabilidade { get; set; } 

    }
}

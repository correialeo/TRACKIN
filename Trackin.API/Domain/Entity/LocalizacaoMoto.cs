using Trackin.API.Domain.Enums;

namespace Trackin.API.Domain.Entity
{
    public class LocalizacaoMoto
    {
        public long Id { get; set; } // PK
        public long MotoId { get; set; } // FK para Moto
        public long PatioId { get; set; } // FK para Pátio
        public double CoordenadaX { get; set; } // Coordenada X no pátio
        public double CoordenadaY { get; set; } // Coordenada Y no pátio
        public DateTime Timestamp { get; set; } // Data/hora da localização
        public MotoStatus Status { get; set; } // Estacionada, Em Movimento
        public FonteDados FonteDados { get; set; } // RFID, VisaoComputacional, Fusao, Manual
        public double Confiabilidade { get; set; } // Porcentagem de confiança na localização

        
        public Moto Moto { get; set; }  
        public Patio Patio { get; set; }  
    }
}

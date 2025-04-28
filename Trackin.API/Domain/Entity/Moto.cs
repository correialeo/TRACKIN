using Trackin.API.Domain.Enums;

namespace Trackin.API.Domain.Entity
{
    public class Moto
    {
        public long Id { get; set; } 
        public string Placa { get; set; } = string.Empty; 
        public ModeloMoto Modelo { get; set; } 
        public int Ano { get; set; } 
        public MotoStatus Status { get; set; } 
        public string RFIDTag { get; set; } = string.Empty; // Identificador único do dispositivo RFID
        public DateTime DataAquisicao { get; set; } // Data de aquisição da moto
        public DateTime? UltimaManutencao { get; set; } // Última manutenção realizada (nullable)
        public string ImagemReferencia { get; set; } = string.Empty; // URL para imagem de referência
        public string CaracteristicasVisuais { get; set; } = string.Empty; // JSON com características visuais
    }
}

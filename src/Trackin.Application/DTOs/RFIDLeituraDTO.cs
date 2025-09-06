using System.ComponentModel.DataAnnotations;

namespace Trackin.Application.DTOs
{
    public class RFIDLeituraDTO
    {
        public string RFID { get; set; } 
        public long SensorId { get; set; }
        [Range(-120, -30, ErrorMessage = "Potência do sinal fora da faixa válida (-120 a -30 dBm)")]
        public double PotenciaSinal { get; set; }
    }
}

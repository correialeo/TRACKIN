using System.Text.Json.Serialization;
using Trackin.API.Domain.Enums;

namespace Trackin.API.Domain.Entity
{
    public class EventoMoto
    {
        public long Id { get; private set; } // PK
        public long MotoId { get; private set; } // FK para Moto
        public EventoMotoTipo Tipo { get; private set; } // Entrada, Saída, Manutenção, etc
        public DateTime Timestamp { get; private set; } = DateTime.UtcNow; 
        public long? UsuarioId { get; private set; } // FK para Usuário (nullable se automatizado)
        public long? SensorId { get; private set; } 
        public long? CameraId { get; private set; } 
        public string? Observacao { get; private set; } = string.Empty; // Observações adicionais
        public FonteDados FonteEvento { get; private set; } // Sistema, Manual, VisaoComputacional, RFID

        public Moto Moto { get; set; }
        public SensorRFID? Sensor { get; set; }
        public Camera? Camera { get; set; } 
        public Usuario? Usuario { get; set; } 

        //construtores
        public EventoMoto() { }
        public EventoMoto(long motoId, EventoMotoTipo tipo, long? usuarioId, long? sensorId, long? cameraiD, string observacao, FonteDados fonteEvento)
        {
            MotoId = motoId;
            Tipo = tipo;
            UsuarioId = usuarioId;
            SensorId = sensorId;
            CameraId = cameraiD;
            Observacao = observacao;
            FonteEvento = fonteEvento;
        }


    }
}

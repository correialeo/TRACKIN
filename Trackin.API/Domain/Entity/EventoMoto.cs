using Trackin.API.Domain.Enums;

namespace Trackin.API.Domain.Entity
{
    public class EventoMoto
    {
        public long Id { get; set; } // PK
        public long MotoId { get; set; } // FK para Moto
        public EventoMotoTipo Tipo { get; set; } // Entrada, Saída, Manutenção, etc
        public DateTime Timestamp { get; set; } // Data/hora do evento
        public int? UsuarioId { get; set; } // FK para Usuário (nullable se automatizado)
        public string Observacao { get; set; } = string.Empty; // Observações adicionais
        public FonteDados FonteEvento { get; set; } // Sistema, Manual, VisaoComputacional, RFID

        public Moto Moto { get; set; } 
    }
}

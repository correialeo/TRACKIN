namespace Trackin.Domain.Entity
{
    public class SensorRFID
    {
        public long Id { get; private set; } 
        public long ZonaPatioId { get; set; } 
        public long PatioId { get; set; } 
        public string Posicao { get; set; } = string.Empty;
        public double PosicaoX { get; set; } 
        public double PosicaoY { get; set; } 
        public double Altura { get; set; } 
        public double AnguloVisao { get; set; }
        public ICollection<EventoMoto> Eventos { get; set; } = new List<EventoMoto>();

        public ZonaPatio? ZonaPatio { get; set; }
        public Patio? Patio { get; set; } 
    }
}

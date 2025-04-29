namespace Trackin.API.DTOs
{
    public class CriarSensorRFIdDTO
    {
        public long ZonaPatioId { get; set; }
        public long PatioId { get; set; }
        public string Posicao { get; set; } = string.Empty;
        public double PosicaoX { get; set; }
        public double PosicaoY { get; set; }
        public double Altura { get; set; }
        public double AnguloVisao { get; set; }
    }
}

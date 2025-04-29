namespace Trackin.API.DTOs
{
    public class RFIDLeituraDTO
    {
        public string RFID { get; set; } 
        public long SensorId { get; set; }
        public double CoordenadaX { get; set; }
        public double CoordenadaY { get; set; }
    }
}

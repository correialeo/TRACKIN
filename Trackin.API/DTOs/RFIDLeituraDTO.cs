namespace Trackin.API.DTOs
{
    public class RFIDLeituraDTO
    {
        public string RFID { get; set; } 
        public long SensorId { get; set; }
        public double PotenciaSinal {  get; set; }
    }
}

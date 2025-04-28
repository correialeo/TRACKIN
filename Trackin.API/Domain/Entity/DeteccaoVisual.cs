namespace Trackin.API.Domain.Entity
{
    public class DeteccaoVisual
    {
        public long Id { get; set; }
        public long? MotoId { get; set; } // FK para Moto (nullable se não identificado)
        public long CameraId { get; set; } // FK para Camera
        public DateTime Timestamp { get; set; } // Data/hora da detecção
        public double CoordenadaXImagem { get; set; } // Coordenada X na imagem capturada
        public double CoordenadaYImagem { get; set; } // Coordenada Y na imagem capturada
        public double CoordenadaXPatio { get; set; } // Coordenada X no pátio
        public double CoordenadaYPatio { get; set; } // Coordenada Y no pátio
        public double Confianca { get; set; } // Porcentagem de confiança na detecção
        public string ImagemCaptura { get; set; } = string.Empty; // URL para imagem capturada (opcional)


        public Moto? Moto { get; set; } 
        public Camera Camera { get; set; } 

    }
}

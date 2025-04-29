using Trackin.API.Domain.Enums;

namespace Trackin.API.Domain.Entity
{
    public class Camera
    {
        public long Id { get; set; }
        public long PatioId { get; set; } // FK para Pátio
        public string Posicao { get; set; } = string.Empty; 
        public double PosicaoX { get; set; } // Coordenada X no pátio
        public double PosicaoY { get; set; } // Coordenada Y no pátio
        public double Altura { get; set; } // Altura de instalação em metros
        public double AnguloVisao { get; set; } // Ângulo de visão em graus
        public CameraStatus Status { get; set; } = CameraStatus.ATIVA; 
        public string URL { get; set; } = string.Empty; // Endpoint para stream de vídeo/imagem
        public EventoMoto[]? Eventos { get; set; } = Array.Empty<EventoMoto>(); 

        public Patio Patio { get; set; }
        public ICollection<DeteccaoVisual> DeteccoesVisuais { get; set; } = new List<DeteccaoVisual>(); 
    }
}

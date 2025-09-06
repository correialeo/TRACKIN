using System.ComponentModel.DataAnnotations;

namespace Trackin.Application.DTOs
{
    public class CriarSensorRFIdDTO
    {
        [Required(ErrorMessage = "O ID da zona do pátio é obrigatório.")]
        [Range(1, long.MaxValue, ErrorMessage = "O ID da zona do pátio deve ser maior que zero.")]
        public long ZonaPatioId { get; set; }

        [Required(ErrorMessage = "O ID do pátio é obrigatório.")]
        [Range(1, long.MaxValue, ErrorMessage = "O ID do pátio deve ser maior que zero.")]
        public long PatioId { get; set; }

        [Required(ErrorMessage = "A posição é obrigatória.")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "A posição deve ter entre 2 e 100 caracteres.")]
        public string Posicao { get; set; } = string.Empty;

        [Range(-10000, 10000, ErrorMessage = "A posição X deve estar entre -10000 e 10000.")]
        public double PosicaoX { get; set; }

        [Range(-10000, 10000, ErrorMessage = "A posição Y deve estar entre -10000 e 10000.")]
        public double PosicaoY { get; set; }

        [Range(0, 100, ErrorMessage = "A altura deve estar entre 0 e 100 metros.")]
        public double Altura { get; set; }

        [Range(0, 360, ErrorMessage = "O ângulo de visão deve estar entre 0 e 360 graus.")]
        public double AnguloVisao { get; set; }
    }
}

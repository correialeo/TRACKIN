using System.ComponentModel.DataAnnotations;
using Trackin.Domain.Enums;

namespace Trackin.Application.DTOs
{
    public class CriarZonaPatioDTO
    {
        [Required(ErrorMessage = "O ID do pátio é obrigatório.")]
        [Range(1, long.MaxValue, ErrorMessage = "O ID do pátio deve ser maior que zero.")]
        public long PatioId { get; set; }

        [Required(ErrorMessage = "O nome é obrigatório.")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "O nome deve ter entre 2 e 100 caracteres.")]
        public string Nome { get; set; } = string.Empty;

        [Required(ErrorMessage = "O tipo de zona é obrigatório.")]
        public TipoZona TipoZona { get; set; }

        [Range(-10000, 10000, ErrorMessage = "A coordenada inicial X deve estar entre -10000 e 10000.")]
        public double CoordenadaInicialX { get; set; }

        [Range(-10000, 10000, ErrorMessage = "A coordenada inicial Y deve estar entre -10000 e 10000.")]
        public double CoordenadaInicialY { get; set; }

        [Range(-10000, 10000, ErrorMessage = "A coordenada final X deve estar entre -10000 e 10000.")]
        public double CoordenadaFinalX { get; set; }

        [Range(-10000, 10000, ErrorMessage = "A coordenada final Y deve estar entre -10000 e 10000.")]
        public double CoordenadaFinalY { get; set; }

        [Required(ErrorMessage = "A cor é obrigatória.")]
        [RegularExpression(@"^#([A-Fa-f0-9]{6}|[A-Fa-f0-9]{3})$", ErrorMessage = "A cor deve estar no formato hexadecimal (#FFFFFF ou #FFF).")]
        public string Cor { get; set; } = string.Empty;
    }
}

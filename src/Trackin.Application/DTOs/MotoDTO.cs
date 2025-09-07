using System.ComponentModel.DataAnnotations;
using Trackin.Domain.Enums;

namespace Trackin.Application.DTOs
{
    public class MotoDTO
    {
        public long Id { get; set; }
        public long PatioId { get; set; } 

        [Required(ErrorMessage = "A placa é obrigatória.")]
        [StringLength(7, MinimumLength = 7, ErrorMessage = "A placa deve ter 7 caracteres.")]
        public string Placa { get; set; }

        [Required(ErrorMessage = "O modelo é obrigatório.")]
        public ModeloMoto Modelo { get; set; }

        [Range(2010, 2026, ErrorMessage = "O ano deve estar entre 2010 e 2026.")]
        public int Ano { get; set; }

        [Required(ErrorMessage = "O RFIDTag é obrigatório.")]
        public string RFIDTag { get; set; }
    }
}

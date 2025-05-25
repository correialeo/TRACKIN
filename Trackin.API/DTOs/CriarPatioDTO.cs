using System.ComponentModel.DataAnnotations;

namespace Trackin.API.DTOs
{
    public class CriarPatioDto
    {
        [Required(ErrorMessage = "O nome é obrigatório.")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "O nome deve ter entre 2 e 100 caracteres.")]
        public string Nome { get; set; } = string.Empty;

        [Required(ErrorMessage = "O endereço é obrigatório.")]
        [StringLength(200, MinimumLength = 5, ErrorMessage = "O endereço deve ter entre 5 e 200 caracteres.")]
        public string Endereco { get; set; } = string.Empty;

        [Required(ErrorMessage = "A cidade é obrigatória.")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "A cidade deve ter entre 2 e 100 caracteres.")]
        public string Cidade { get; set; } = string.Empty;

        [Required(ErrorMessage = "O estado é obrigatório.")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "O estado deve ter entre 2 e 50 caracteres.")]
        public string Estado { get; set; } = string.Empty;

        [Required(ErrorMessage = "O país é obrigatório.")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "O país deve ter entre 2 e 50 caracteres.")]
        public string Pais { get; set; } = string.Empty;

        [Range(1, 10000, ErrorMessage = "A dimensão X deve estar entre 1 e 10000 metros.")]
        public double DimensaoX { get; set; }

        [Range(1, 10000, ErrorMessage = "A dimensão Y deve estar entre 1 e 10000 metros.")]
        public double DimensaoY { get; set; }

        [StringLength(500, ErrorMessage = "A planta baixa deve ter no máximo 500 caracteres.")]
        public string PlantaBaixa { get; set; } = string.Empty;
    }
}

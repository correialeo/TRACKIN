using System.ComponentModel.DataAnnotations;

namespace Trackin.Application.DTOs
{
    public class PaginacaoDTO
    {
        [Range(1, int.MaxValue, ErrorMessage = "A página deve ser maior que 0.")]
        public int PageNumber { get; set; } = 1;

        [Range(1, 100, ErrorMessage = "O tamanho da página deve estar entre 1 e 100.")]
        public int PageSize { get; set; } = 10;

        public string? Ordering { get; set; }
        public bool DescendingOrder { get; set; } = false;
    }
}

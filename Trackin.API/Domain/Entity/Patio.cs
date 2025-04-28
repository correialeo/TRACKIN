namespace Trackin.API.Domain.Entity
{
    public class Patio
    {
        public long Id { get; set; } 
        public string Nome { get; set; } = string.Empty; // Nome do pátio
        public string Endereco { get; set; } = string.Empty; // Endereço do pátio
        public string Cidade { get; set; } = string.Empty; // Cidade
        public string Estado { get; set; } = string.Empty; // Estado
        public string Pais { get; set; } = string.Empty; // País (Brasil/México)
        public double DimensaoX { get; set; } // Largura em metros
        public double DimensaoY { get; set; } // Comprimento em metros
        public string PlantaBaixa { get; set; } = string.Empty; // URL para imagem da planta baixa
    }
}

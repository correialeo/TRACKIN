namespace Trackin.API.DTOs
{
    public class CriarPatioDto
    {
        public string Nome { get; set; } = string.Empty;
        public string Endereco { get; set; } = string.Empty;
        public string Cidade { get; set; } = string.Empty;
        public string Estado { get; set; } = string.Empty;
        public string Pais { get; set; } = string.Empty;
        public double DimensaoX { get; set; }
        public double DimensaoY { get; set; }
        public string PlantaBaixa { get; set; } = string.Empty;
    }
}

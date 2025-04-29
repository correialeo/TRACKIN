using Trackin.API.Domain.Enums;

namespace Trackin.API.Domain.Entity
{
    public class Moto
    {
        public long Id { get; private set; } 
        public long PatioId { get; private set; }
        public string Placa { get; private set; } = string.Empty;
        public ModeloMoto Modelo { get; private set; }
        public int Ano { get; private set; }
        public MotoStatus Status { get; private set; } = MotoStatus.DISPONIVEL;
        public string RFIDTag { get; private set; } = string.Empty;
        public DateTime? UltimaManutencao { get; private set; }
        public string ImagemReferencia { get; private set; } = string.Empty;
        public string CaracteristicasVisuais { get; private set; } = string.Empty;


        public Patio Patio { get; set; }
        public ICollection<EventoMoto> Eventos { get; set; } = new List<EventoMoto>(); 
        public ICollection<LocalizacaoMoto> Localizacoes { get; set; } = new List<LocalizacaoMoto>(); 
        public ICollection<DeteccaoVisual> DeteccoesVisuais { get; set; } = new List<DeteccaoVisual>(); // Detecções visuais associadas à moto


        public Moto(){}

        public Moto(long patioId, string placa, ModeloMoto modelo, int ano, string rfidTag)
        {
            PatioId = patioId;
            Placa = placa;
            Modelo = modelo;
            Ano = ano;
            RFIDTag = rfidTag;
        }

        public void AlterarStatus(MotoStatus novoStatus)
        {
            if (novoStatus != Status)
            {
                Status = novoStatus;
            }
        }

        public void RegistrarManutencao(DateTime data)
        {
            UltimaManutencao = data;
        }

        public void AtualizarImagemReferencia(string imagem)
        {
            ImagemReferencia = imagem;
        }
    }
}

using Trackin.Domain.Enums;
using Trackin.Domain.ValueObjects;

namespace Trackin.Domain.Entity
{
    public class ZonaPatio
    {
        public long Id { get; private set; }
        public long PatioId { get; private set; }
        public string Nome { get; private set; } = string.Empty;
        public TipoZona TipoZona { get; private set; }
        public Coordenada PontoInicial { get; private set; }
        public Coordenada PontoFinal { get; private set; }
        public string Cor { get; private set; } = string.Empty;

        public Patio Patio { get; private set; }
        private readonly List<SensorRFID> _sensoresRFID = new();
        public IReadOnlyCollection<SensorRFID> SensoresRFID => _sensoresRFID.AsReadOnly();

        protected ZonaPatio() { }

        public ZonaPatio(long patioId, string nome, TipoZona tipoZona, Coordenada pontoInicial, Coordenada pontoFinal, string cor)
        {
            ValidarParametrosZona(nome, pontoInicial, pontoFinal, cor);

            PatioId = patioId;
            Nome = nome;
            TipoZona = tipoZona;
            PontoInicial = pontoInicial;
            PontoFinal = pontoFinal;
            Cor = cor;
        }

        // COMPORTAMENTOS RICOS
        public bool ContemPosicao(Coordenada posicao)
        {
            if (posicao == null)
                return false;

            return posicao.EstaDentroDoRetangulo(PontoInicial, PontoFinal);
        }

        public double CalcularArea()
        {
            var largura = Math.Abs(PontoFinal.X - PontoInicial.X);
            var altura = Math.Abs(PontoFinal.Y - PontoInicial.Y);
            return largura * altura;
        }

        public Coordenada ObterCentroZona()
        {
            var centroX = (PontoInicial.X + PontoFinal.X) / 2;
            var centroY = (PontoInicial.Y + PontoFinal.Y) / 2;
            return new Coordenada(centroX, centroY);
        }

        public bool TemSobreposicaoCom(Coordenada outroPontoInicial, Coordenada outroPontoFinal)
        {
            if (outroPontoInicial == null || outroPontoFinal == null)
                return false;

            var minX1 = Math.Min(PontoInicial.X, PontoFinal.X);
            var maxX1 = Math.Max(PontoInicial.X, PontoFinal.X);
            var minY1 = Math.Min(PontoInicial.Y, PontoFinal.Y);
            var maxY1 = Math.Max(PontoInicial.Y, PontoFinal.Y);

            var minX2 = Math.Min(outroPontoInicial.X, outroPontoFinal.X);
            var maxX2 = Math.Max(outroPontoInicial.X, outroPontoFinal.X);
            var minY2 = Math.Min(outroPontoInicial.Y, outroPontoFinal.Y);
            var maxY2 = Math.Max(outroPontoInicial.Y, outroPontoFinal.Y);

            return !(maxX1 < minX2 || maxX2 < minX1 || maxY1 < minY2 || maxY2 < minY1);
        }

        public bool EhZonaDeEstacionamento()
        {
            return TipoZona == TipoZona.ZONA_DE_ESTACIONAMENTO;
        }

        public bool EhZonaDeManutencao()
        {
            return TipoZona == TipoZona.ZONA_DE_MANUTENCAO;
        }

        public void AlterarCor(string novaCor)
        {
            if (string.IsNullOrWhiteSpace(novaCor))
                throw new ArgumentException("Cor não pode ser vazia", nameof(novaCor));

            Cor = novaCor;
        }

        public void RedimensionarZona(Coordenada novoPontoInicial, Coordenada novoPontoFinal)
        {
            if (novoPontoInicial == null || novoPontoFinal == null)
                throw new ArgumentNullException("Pontos da zona não podem ser nulos");

            PontoInicial = novoPontoInicial;
            PontoFinal = novoPontoFinal;
        }

        private void ValidarParametrosZona(string nome, Coordenada pontoInicial, Coordenada pontoFinal, string cor)
        {
            if (string.IsNullOrWhiteSpace(nome))
                throw new ArgumentException("Nome da zona não pode ser vazio", nameof(nome));

            if (pontoInicial == null)
                throw new ArgumentNullException(nameof(pontoInicial));

            if (pontoFinal == null)
                throw new ArgumentNullException(nameof(pontoFinal));

            if (string.IsNullOrWhiteSpace(cor))
                throw new ArgumentException("Cor não pode ser vazia", nameof(cor));
        }
    }
}

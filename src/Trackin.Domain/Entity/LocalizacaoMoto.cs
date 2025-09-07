using Trackin.Domain.Enums;
using Trackin.Domain.ValueObjects;

namespace Trackin.Domain.Entity
{
    public class LocalizacaoMoto
    {
        public long Id { get; private set; }
        public long MotoId { get; private set; }
        public long PatioId { get; private set; }
        public Coordenada Posicao { get; private set; }
        public DateTime Timestamp { get; private set; }
        public MotoStatus Status { get; private set; }
        public FonteDados FonteDados { get; private set; }
        public double Confiabilidade { get; private set; }

        public Moto Moto { get; private set; }
        public Patio Patio { get; private set; }

        protected LocalizacaoMoto() { }

        public LocalizacaoMoto(long motoId, long patioId, Coordenada posicao, MotoStatus status, FonteDados fonteDados, double confiabilidade)
        {
            ValidarParametrosLocalizacao(posicao, confiabilidade);

            MotoId = motoId;
            PatioId = patioId;
            Posicao = posicao;
            Status = status;
            FonteDados = fonteDados;
            Confiabilidade = confiabilidade;
            Timestamp = DateTime.UtcNow;
        }

        public bool EhLocalizacaoConfiavel(double limiteConfiabilidade = 0.8)
        {
            return Confiabilidade >= limiteConfiabilidade;
        }

        public double DistanciaPara(LocalizacaoMoto outraLocalizacao)
        {
            if (outraLocalizacao == null)
                throw new ArgumentNullException(nameof(outraLocalizacao));

            return Posicao.DistanciaEuclidiana(outraLocalizacao.Posicao);
        }

        public bool MotoMoveuPara(LocalizacaoMoto novaLocalizacao, double distanciaMinima = 1.0)
        {
            if (novaLocalizacao == null)
                return false;

            return DistanciaPara(novaLocalizacao) >= distanciaMinima;
        }

        public TimeSpan TempoNaPosicao()
        {
            return DateTime.UtcNow - Timestamp;
        }

        public bool EstaDisponivelHaMuitoTempo(TimeSpan tempoLimite)
        {
            return Status == MotoStatus.DISPONIVEL && TempoNaPosicao() > tempoLimite;
        }

        public bool PosicaoEstaEmZona(ZonaPatio zona)
        {
            if (zona == null)
                return false;

            var pontoInicial = zona.PontoInicial;
            var pontoFinal = zona.PontoFinal;

            return Posicao.EstaDentroDoRetangulo(pontoInicial, pontoFinal);
        }

        private void ValidarParametrosLocalizacao(Coordenada posicao, double confiabilidade)
        {
            if (posicao == null)
                throw new ArgumentNullException(nameof(posicao));

            if (confiabilidade < 0 || confiabilidade > 1)
                throw new ArgumentException("Confiabilidade deve estar entre 0 e 1", nameof(confiabilidade));
        }
    }
}

using Trackin.Domain.Enums;
using Trackin.Domain.ValueObjects;

namespace Trackin.Domain.Entity
{
    public class SensorRFID
    {
        public long Id { get; private set; }
        public long ZonaPatioId { get; private set; }
        public long PatioId { get; private set; }
        public string Posicao { get; private set; } = string.Empty;
        public Coordenada PosicaoSensor { get; private set; }
        public double Altura { get; private set; }
        public double AnguloVisao { get; private set; }
        public bool Ativo { get; private set; } = true;
        public DateTime UltimaLeitura { get; private set; }

        public ZonaPatio ZonaPatio { get; private set; }
        public Patio Patio { get; private set; }
        private readonly List<EventoMoto> _eventos = new();
        public IReadOnlyCollection<EventoMoto> Eventos => _eventos.AsReadOnly();

        protected SensorRFID() { }

        public SensorRFID(long zonaPatioId, long patioId, string posicao, Coordenada posicaoSensor, double altura, double anguloVisao)
        {
            ValidarParametrosSensor(posicao, posicaoSensor, altura, anguloVisao);

            ZonaPatioId = zonaPatioId;
            PatioId = patioId;
            Posicao = posicao;
            PosicaoSensor = posicaoSensor;
            Altura = altura;
            AnguloVisao = anguloVisao;
            Ativo = true;
        }

        public void AtivarSensor()
        {
            if (Ativo)
                throw new InvalidOperationException("Sensor já está ativo");

            Ativo = true;
        }

        public void DesativarSensor()
        {
            if (!Ativo)
                throw new InvalidOperationException("Sensor já está inativo");

            Ativo = false;
        }

        public bool PodeLerRFID()
        {
            return Ativo;
        }

        public void RegistrarLeitura()
        {
            if (!Ativo)
                throw new InvalidOperationException("Não é possível registrar leitura com sensor inativo");

            UltimaLeitura = DateTime.UtcNow;
        }

        public TimeSpan TempoSemLeitura()
        {
            return DateTime.UtcNow - UltimaLeitura;
        }

        public bool EstaComProblema(TimeSpan tempoLimiteSemLeitura)
        {
            return Ativo && TempoSemLeitura() > tempoLimiteSemLeitura;
        }

        public double DistanciaPara(Coordenada ponto)
        {
            return PosicaoSensor.DistanciaEuclidiana(ponto);
        }

        public bool PontoEstaNoAlcance(Coordenada ponto, double raioMaximo)
        {
            return DistanciaPara(ponto) <= raioMaximo;
        }

        public void AdicionarEvento(EventoMoto evento)
        {
            if (evento == null)
                throw new ArgumentNullException(nameof(evento));

            if (evento.SensorId != Id)
                throw new ArgumentException("Evento não pertence a este sensor");

            _eventos.Add(evento);
            RegistrarLeitura();
        }

        public void AtualizarPosicao(Coordenada novaPosicao, double novaAltura = -1)
        {
            if (novaPosicao == null)
                throw new ArgumentNullException(nameof(novaPosicao));

            PosicaoSensor = novaPosicao;

            if (novaAltura >= 0)
                Altura = novaAltura;
        }

        public IEnumerable<EventoMoto> ObterEventosRecentes(TimeSpan periodo)
        {
            var dataLimite = DateTime.UtcNow - periodo;
            return _eventos.Where(e => e.Timestamp >= dataLimite);
        }

        public int ContarEventosPorTipo(EventoMotoTipo tipo, TimeSpan periodo)
        {
            return ObterEventosRecentes(periodo).Count(e => e.Tipo == tipo);
        }

        private void ValidarParametrosSensor(string posicao, Coordenada posicaoSensor, double altura, double anguloVisao)
        {
            if (string.IsNullOrWhiteSpace(posicao))
                throw new ArgumentException("Posição não pode ser vazia", nameof(posicao));

            if (posicaoSensor == null)
                throw new ArgumentNullException(nameof(posicaoSensor));

            if (altura < 0)
                throw new ArgumentException("Altura deve ser maior ou igual a zero", nameof(altura));

            if (anguloVisao <= 0 || anguloVisao > 360)
                throw new ArgumentException("Ângulo de visão deve estar entre 0 e 360 graus", nameof(anguloVisao));
        }
    }
}

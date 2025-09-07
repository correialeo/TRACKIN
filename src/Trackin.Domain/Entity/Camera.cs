using Trackin.Domain.Enums;
using Trackin.Domain.ValueObjects;

namespace Trackin.Domain.Entity
{
    public class Camera
    {
        public long Id { get; private set; }
        public long PatioId { get; private set; }
        public string Posicao { get; private set; } = string.Empty;
        public Coordenada PosicaoPatio { get; private set; }
        public double Altura { get; private set; }
        public double AnguloVisao { get; private set; }
        public CameraStatus Status { get; private set; } = CameraStatus.ATIVA;
        public string URL { get; private set; } = string.Empty;

        public Patio Patio { get; private set; }
        private readonly List<EventoMoto> _eventos = new();
        private readonly List<DeteccaoVisual> _deteccoesVisuais = new();

        public IReadOnlyCollection<EventoMoto> Eventos => _eventos.AsReadOnly();
        public IReadOnlyCollection<DeteccaoVisual> DeteccoesVisuais => _deteccoesVisuais.AsReadOnly();

        protected Camera() { }

        public Camera(long patioId, string posicao, Coordenada posicaoPatio, double altura, double anguloVisao, string url)
        {
            ValidarParametrosConstucao(posicao, posicaoPatio, altura, anguloVisao, url);

            PatioId = patioId;
            Posicao = posicao;
            PosicaoPatio = posicaoPatio;
            Altura = altura;
            AnguloVisao = anguloVisao;
            URL = url;
            Status = CameraStatus.ATIVA;
        }

        // COMPORTAMENTOS RICOS
        public void AtivarCamera()
        {
            if (Status == CameraStatus.ATIVA)
                throw new InvalidOperationException("Camera já está ativa");

            Status = CameraStatus.ATIVA;
        }

        public void DesativarCamera()
        {
            if (Status == CameraStatus.INATIVA)
                throw new InvalidOperationException("Camera já está inativa");

            Status = CameraStatus.INATIVA;
        }

        public void MarcarManutencao()
        {
            Status = CameraStatus.MANUTENCAO;
        }

        public bool PodeCapturarImagem()
        {
            return Status == CameraStatus.ATIVA && !string.IsNullOrWhiteSpace(URL);
        }

        public void AtualizarPosicao(Coordenada novaPosicao, double novaAltura = -1)
        {
            if (novaPosicao == null)
                throw new ArgumentNullException(nameof(novaPosicao));

            PosicaoPatio = novaPosicao;

            if (novaAltura >= 0)
                Altura = novaAltura;
        }

        public double DistanciaPara(Coordenada ponto)
        {
            return PosicaoPatio.DistanciaEuclidiana(ponto);
        }

        public bool PontoEstaNoAlcance(Coordenada ponto, double raioMaximo)
        {
            return DistanciaPara(ponto) <= raioMaximo;
        }

        public void AdicionarDeteccao(DeteccaoVisual deteccao)
        {
            if (deteccao == null)
                throw new ArgumentNullException(nameof(deteccao));

            if (deteccao.CameraId != Id)
                throw new ArgumentException("Detecção não pertence a esta camera");

            _deteccoesVisuais.Add(deteccao);
        }

        private void ValidarParametrosConstucao(string posicao, Coordenada posicaoPatio, double altura, double anguloVisao, string url)
        {
            if (string.IsNullOrWhiteSpace(posicao))
                throw new ArgumentException("Posição não pode ser vazia", nameof(posicao));

            if (posicaoPatio == null)
                throw new ArgumentNullException(nameof(posicaoPatio));

            if (altura < 0)
                throw new ArgumentException("Altura deve ser maior que zero", nameof(altura));

            if (anguloVisao <= 0 || anguloVisao > 360)
                throw new ArgumentException("Ângulo de visão deve estar entre 0 e 360 graus", nameof(anguloVisao));

            if (string.IsNullOrWhiteSpace(url))
                throw new ArgumentException("URL não pode ser vazia", nameof(url));
        }
    }
}

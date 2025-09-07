using Trackin.Domain.ValueObjects;

namespace Trackin.Domain.Entity
{
    public class DeteccaoVisual
    {
        public long Id { get; private set; }
        public long? MotoId { get; private set; }
        public long CameraId { get; private set; }
        public DateTime Timestamp { get; private set; }
        public Coordenada PosicaoImagem { get; private set; }
        public Coordenada PosicaoPatio { get; private set; }
        public double Confianca { get; private set; }
        public string ImagemCaptura { get; private set; } = string.Empty;

        public Moto? Moto { get; private set; }
        public Camera Camera { get; private set; }

        protected DeteccaoVisual() { }

        public DeteccaoVisual(long cameraId, Coordenada posicaoImagem, Coordenada posicaoPatio, double confianca, long? motoId = null, string imagemCaptura = "")
        {
            ValidarParametrosDeteccao(posicaoImagem, posicaoPatio, confianca);

            CameraId = cameraId;
            MotoId = motoId;
            PosicaoImagem = posicaoImagem;
            PosicaoPatio = posicaoPatio;
            Confianca = confianca;
            ImagemCaptura = imagemCaptura ?? string.Empty;
            Timestamp = DateTime.UtcNow;
        }

        public bool EhDeteccaoConfiavel(double limiteConfianca = 0.7)
        {
            return Confianca >= limiteConfianca;
        }

        public bool MotoFoiIdentificada()
        {
            return MotoId.HasValue;
        }

        public void AssociarMoto(long motoId)
        {
            if (motoId <= 0)
                throw new ArgumentException("ID da moto deve ser válido", nameof(motoId));

            MotoId = motoId;
        }

        public void RemoverAssociacaoMoto()
        {
            MotoId = null;
        }

        public bool EstaProximaDe(DeteccaoVisual outraDeteccao, double distanciaMaxima)
        {
            if (outraDeteccao == null)
                return false;

            return PosicaoPatio.DistanciaEuclidiana(outraDeteccao.PosicaoPatio) <= distanciaMaxima;
        }

        public TimeSpan TempoDecorridoDesdeDeteccao()
        {
            return DateTime.UtcNow - Timestamp;
        }

        private void ValidarParametrosDeteccao(Coordenada posicaoImagem, Coordenada posicaoPatio, double confianca)
        {
            if (posicaoImagem == null)
                throw new ArgumentNullException(nameof(posicaoImagem));

            if (posicaoPatio == null)
                throw new ArgumentNullException(nameof(posicaoPatio));

            if (confianca < 0 || confianca > 1)
                throw new ArgumentException("Confiança deve estar entre 0 e 1", nameof(confianca));
        }
    }
}

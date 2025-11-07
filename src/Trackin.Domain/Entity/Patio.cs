using Trackin.Domain.Enums;
using Trackin.Domain.ValueObjects;

namespace Trackin.Domain.Entity
{
    /// <summary>
    /// Agregado raiz do domínio Patio.
    /// Controla todas as operações relacionadas ao pátio e suas entidades filhas.
    /// </summary>
    public class Patio
    {
        public long Id { get; private set; }
        public string Nome { get; private set; } = string.Empty;
        public string Endereco { get; private set; } = string.Empty;
        public string Cidade { get; private set; } = string.Empty;
        public string Estado { get; private set; } = string.Empty;
        public string Pais { get; private set; } = string.Empty;
        public Coordenada Dimensoes { get; private set; }
        public string PlantaBaixa { get; private set; } = string.Empty;

        private readonly List<Camera> _cameras = new();
        private readonly List<ZonaPatio> _zonas = new();
        private readonly List<SensorRFID> _sensoresRFID = new();
        private readonly List<Usuario> _usuarios = new();
        private readonly List<LocalizacaoMoto> _localizacoes = new();
        private readonly List<Moto> _motos = new();

        public IReadOnlyCollection<Camera> Cameras => _cameras.AsReadOnly();
        public IReadOnlyCollection<ZonaPatio> Zonas => _zonas.AsReadOnly();
        public IReadOnlyCollection<SensorRFID> SensoresRFID => _sensoresRFID.AsReadOnly();
        public IReadOnlyCollection<Usuario> Usuarios => _usuarios.AsReadOnly();
        public IReadOnlyCollection<LocalizacaoMoto> Localizacoes => _localizacoes.AsReadOnly();
        public IReadOnlyCollection<Moto> Motos => _motos.AsReadOnly();

        protected Patio() { }

        public Patio(string nome, string endereco, string cidade, string estado, string pais, double largura, double comprimento)
        {
            ValidarParametrosPatio(nome, endereco, cidade, estado, pais, largura, comprimento);

            Nome = nome;
            Endereco = endereco;
            Cidade = cidade;
            Estado = estado;
            Pais = pais;
            Dimensoes = new Coordenada(largura, comprimento);
        }

        public Camera AdicionarCamera(string posicao, Coordenada posicaoPatio, double altura, double anguloVisao, string url)
        {
            ValidarCoordenadaDentroDoPatio(posicaoPatio);

            Camera camera = new Camera(Id, posicao, posicaoPatio, altura, anguloVisao, url);
            _cameras.Add(camera);

            return camera;
        }

        public ZonaPatio CriarZona(string nome, TipoZona tipoZona, Coordenada pontoInicial, Coordenada pontoFinal, string cor)
        {
            ValidarCoordenadaDentroDoPatio(pontoInicial);
            ValidarCoordenadaDentroDoPatio(pontoFinal);
            ValidarSobreposicaoZonas(pontoInicial, pontoFinal);

            ZonaPatio zona = new ZonaPatio(Id, nome, tipoZona, pontoInicial, pontoFinal, cor);
            _zonas.Add(zona);

            return zona;
        }

        public SensorRFID InstalarSensorRFID(long zonaId, string posicao, Coordenada posicaoSensor, double altura, double anguloVisao)
        {
            ValidarCoordenadaDentroDoPatio(posicaoSensor);

            ZonaPatio? zona = _zonas.FirstOrDefault(z => z.Id == zonaId);
            if (zona == null)
                throw new ArgumentException("Zona não encontrada neste pátio");

            SensorRFID sensor = new SensorRFID(zonaId, Id, posicao, posicaoSensor, altura, anguloVisao);
            _sensoresRFID.Add(sensor);

            return sensor;
        }

        public void AdicionarUsuario(Usuario usuario)
        {
            if (usuario == null)
                throw new ArgumentNullException(nameof(usuario));

            if (_usuarios.Any(u => u.Email == usuario.Email))
                throw new InvalidOperationException("Já existe um usuário com este email neste pátio");

            _usuarios.Add(usuario);
        }

        public Moto AdicionarMoto(string placa, ModeloMoto modelo, int ano, string rfidTag)
        {
            if (string.IsNullOrWhiteSpace(placa))
                throw new ArgumentException("Placa não pode ser vazia", nameof(placa));

            if (string.IsNullOrWhiteSpace(rfidTag))
                throw new ArgumentException("RFID Tag não pode ser vazia", nameof(rfidTag));

            if (_motos.Any(m => m.Placa == placa))
                throw new InvalidOperationException("Já existe uma moto com esta placa neste pátio");

            if (_motos.Any(m => m.RFIDTag == rfidTag))
                throw new InvalidOperationException("Já existe uma moto com este RFID Tag neste pátio");

            Moto moto = new Moto(Id, placa, modelo, ano, rfidTag);
            _motos.Add(moto);

            return moto;
        }

        public void RemoverMoto(long motoId)
        {
            Moto? moto = _motos.FirstOrDefault(m => m.Id == motoId);
            if (moto == null)
                throw new InvalidOperationException("Moto não encontrada neste pátio");

            _motos.Remove(moto);
        }

        public Moto? ObterMoto(long motoId)
        {
            return _motos.FirstOrDefault(m => m.Id == motoId);
        }

        public IEnumerable<Moto> ObterMotosPorStatus(MotoStatus status)
        {
            return _motos.Where(m => m.Status == status);
        }

        public bool CoordenadaEstaValida(Coordenada coordenada)
        {
            if (coordenada == null)
                return false;

            return coordenada.X >= 0 && coordenada.X <= Dimensoes.X &&
                   coordenada.Y >= 0 && coordenada.Y <= Dimensoes.Y;
        }

        public IEnumerable<Camera> ObterCamerasProximasDe(Coordenada posicao, double raio)
        {
            return _cameras.Where(c => c.PontoEstaNoAlcance(posicao, raio));
        }

        public ZonaPatio? ObterZonaPorPosicao(Coordenada posicao)
        {
            return _zonas.FirstOrDefault(z => z.ContemPosicao(posicao));
        }

        public int ObterTotalMotosDisponiveis()
        {
            return _localizacoes
                .Where(l => l.Status == MotoStatus.DISPONIVEL)
                .GroupBy(l => l.MotoId)
                .Count();
        }

        public double CalcularTaxaOcupacao()
        {
            int totalZonasEstacionamento = _zonas.Count(z => z.TipoZona == TipoZona.ZONA_DE_ESTACIONAMENTO);
            if (totalZonasEstacionamento == 0) return 0;

            int motosEstacionadas = ObterTotalMotosDisponiveis();
            return (double)motosEstacionadas / totalZonasEstacionamento;
        }

        private void ValidarParametrosPatio(string nome, string endereco, string cidade, string estado, string pais, double largura, double comprimento)
        {
            if (string.IsNullOrWhiteSpace(nome))
                throw new ArgumentException("Nome do pátio não pode ser vazio", nameof(nome));

            if (string.IsNullOrWhiteSpace(endereco))
                throw new ArgumentException("Endereço não pode ser vazio", nameof(endereco));

            if (largura <= 0 || comprimento <= 0)
                throw new ArgumentException("Dimensões do pátio devem ser maiores que zero");
        }

        private void ValidarCoordenadaDentroDoPatio(Coordenada coordenada)
        {
            if (!CoordenadaEstaValida(coordenada))
                throw new ArgumentException($"Coordenada {coordenada} está fora dos limites do pátio ({Dimensoes})");
        }

        private void ValidarSobreposicaoZonas(Coordenada pontoInicial, Coordenada pontoFinal)
        {
            foreach (ZonaPatio zona in _zonas)
            {
                if (zona.TemSobreposicaoCom(pontoInicial, pontoFinal))
                    throw new InvalidOperationException($"Nova zona se sobrepõe com a zona '{zona.Nome}'");
            }
        }
    }
}

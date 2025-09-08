
using Microsoft.Extensions.Logging;
using Trackin.Application.Common;
using Trackin.Application.DTOs;
using Trackin.Application.Interfaces;
using Trackin.Domain.Entity;
using Trackin.Domain.Enums;
using Trackin.Domain.Interfaces;
using Trackin.Domain.ValueObjects;

namespace Trackin.Application.Services
{
    public class RFIDService : IRFIDService
    {
        private const string MotoNaoExiste = "Moto não encontrada";
        private const string SensorNaoExiste = "Sensor RFID não encontrado";
        private const string SensorSemZona = "Sensor não está associado a nenhuma zona";

        private readonly IMotoRepository _motoRepository;
        private readonly ISensorRFIDRepository _sensorRepository;
        private readonly IEventoMotoRepository _eventoRepository;
        private readonly ILocalizacaoMotoRepository _localizacaoRepository;
        private readonly ILogger<RFIDService> _logger;

        public RFIDService(
            IMotoRepository motoRepository,
            ISensorRFIDRepository sensorRepository,
            IEventoMotoRepository eventoRepository,
            ILocalizacaoMotoRepository localizacaoRepository,
            ILogger<RFIDService> logger)
        {
            _motoRepository = motoRepository;
            _sensorRepository = sensorRepository;
            _eventoRepository = eventoRepository;
            _localizacaoRepository = localizacaoRepository;
            _logger = logger;
        }
        
        private async Task<Moto?> ObterMoto(string rfid) => await _motoRepository.GetByRFIDTagAsync(rfid);

        private async Task<SensorRFID?> ObterSensor(long sensorId) => await _sensorRepository.GetSensorWithZonaAndPatioAsync(sensorId);

        private ServiceResponse<T> Sucesso<T>(T data, string message = "") => new() { Success = true, Data = data, Message = message };

        private ServiceResponse<T> Erro<T>(string message) => new() { Success = false, Message = message };


        public async Task<ServiceResponse<LocalizacaoMotoDTO>> ProcessarLeituraRFID(RFIDLeituraDTO leitura)
        {
             try
            {
                Moto? moto = await ObterMoto(leitura.RFID);
                if (moto == null) return Erro<LocalizacaoMotoDTO>(MotoNaoExiste);

                SensorRFID? sensor = await ObterSensor(leitura.SensorId);
                if (sensor == null) return Erro<LocalizacaoMotoDTO>(SensorNaoExiste);
                if (sensor.ZonaPatio == null) return Erro<LocalizacaoMotoDTO>(SensorSemZona);

                (double x, double y) = CalcularCoordenadas(sensor, leitura.PotenciaSinal);
                EventoMotoTipo tipoEvento = DeterminarTipoEvento(sensor.ZonaPatio.TipoZona);
                MotoStatus status = DeterminarStatusMoto(tipoEvento);

                EventoMoto evento = new(moto.Id, tipoEvento, null, leitura.SensorId, null, null, FonteDados.RFID);
                LocalizacaoMoto localizacao = new(moto.Id, sensor.ZonaPatio.PatioId, new Coordenada(x, y), status, FonteDados.RFID, CalcularConfiabilidade(sensor));

                try { moto.AlterarStatus(status); await _motoRepository.UpdateAsync(moto); }
                catch (Exception ex) { _logger.LogWarning($"Não foi possível atualizar o status da moto: {ex.Message}"); }

                await Task.WhenAll(_eventoRepository.AddAsync(evento), _localizacaoRepository.AddAsync(localizacao));
                await _eventoRepository.SaveChangesAsync();

                var localizacaoDto = new LocalizacaoMotoDTO
                {
                    Id = localizacao.Id,
                    MotoId = moto.Id,
                    PatioId = sensor.ZonaPatio.PatioId,
                    CoordenadaX = x,
                    CoordenadaY = y,
                    Timestamp = DateTime.UtcNow,
                    Status = status,
                    FonteDados = FonteDados.RFID,
                    Confiabilidade = CalcularConfiabilidade(sensor)
                };

                return Sucesso(localizacaoDto, "Leitura RFID processada com sucesso");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Erro ao processar leitura RFID {leitura.RFID}: {ex.Message}");
                return Erro<LocalizacaoMotoDTO>($"Erro ao processar leitura RFID: {ex.Message}");
            }
        }

        private EventoMotoTipo DeterminarTipoEvento(TipoZona tipoZona)
        {
            return tipoZona switch
            {
                TipoZona.ZONA_DE_MANUTENCAO => EventoMotoTipo.MANUTENCAO,
                TipoZona.ZONA_DE_ESTACIONAMENTO => EventoMotoTipo.PARADA,
                TipoZona.ZONA_DE_ENTRADA => EventoMotoTipo.ENTRADA,
                TipoZona.ZONA_DE_SAIDA => EventoMotoTipo.SAIDA,
                _ => EventoMotoTipo.ENTRADA
            };
        }

        private MotoStatus DeterminarStatusMoto(EventoMotoTipo tipoEvento)
        {
            return tipoEvento switch
            {
                EventoMotoTipo.ENTRADA => MotoStatus.DISPONIVEL,
                EventoMotoTipo.SAIDA => MotoStatus.ALUGADA,
                EventoMotoTipo.MANUTENCAO => MotoStatus.EM_MANUTENCAO,
                EventoMotoTipo.PARADA => MotoStatus.DISPONIVEL,
                _ => MotoStatus.DISPONIVEL
            };
        }

        private (double x, double y) CalcularCoordenadas(SensorRFID sensor, double potenciaSinal)
        {
            double distancia = CalcularDistanciaPorPotencia(potenciaSinal);

            double angulo = ObterAnguloAleatorio(); // simula direcao do sinal
            double offsetX = distancia * Math.Cos(angulo);
            double offsetY = distancia * Math.Sin(angulo);

            return (sensor.PosicaoSensor.X + offsetX, sensor.PosicaoSensor.Y + offsetY);
        }

        private double CalcularConfiabilidade(SensorRFID sensor)
        {
            // Lógica para calcular a confiabilidade baseada em características do sensor
            // Por exemplo, potência do sinal, histórico de precisão, etc.
            // Por agora, retorna um valor fixo
            return 0.8;
        }

        private async Task NotificarAtualizacaoLocalizacao(LocalizacaoMoto localizacao)
        {
            // Implementação de notificação via SignalR ou outro mecanismo
            // Exemplo:
            // await _hubContext.Clients.Group($"patio_{localizacao.PatioId}").SendAsync("AtualizacaoLocalizacao", localizacao);
        }

        private double CalcularDistanciaPorPotencia(double potenciaSinal)
        {
            // pot padrao: -30 dBm (perto) a -90 dBm (longe)
            const double potenciaMaxima = -30; // dBm a 0 metros
            const double potenciaMinima = -90; // dBm a 20 metros

            // mapeamento linear (adaptar para modelo de propagação real)
            double distancia = (potenciaSinal - potenciaMaxima) / (potenciaMinima - potenciaMaxima) * 20;

            return Math.Clamp(distancia, 0, 20); // limitado a 20 metros
        }

        private double ObterAnguloAleatorio()
        {
            // simula direcao aleatória (em radianos)
            Random rand = new();
            return rand.NextDouble() * 2 * Math.PI;
        }
    }
}
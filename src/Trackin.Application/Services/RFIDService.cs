
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

        public async Task<ServiceResponse<LocalizacaoMotoDTO>> ProcessarLeituraRFID(RFIDLeituraDTO leitura)
        {
            try
            {
                Moto? moto = await _motoRepository.GetByRFIDTagAsync(leitura.RFID);
                if (moto == null)
                {
                    _logger.LogWarning($"RFID não encontrado: {leitura.RFID}");
                    return new ServiceResponse<LocalizacaoMotoDTO>
                    {
                        Success = false,
                        Message = $"Moto com RFID {leitura.RFID} não encontrada"
                    };
                }

                SensorRFID? sensor = await _sensorRepository.GetSensorWithZonaAndPatioAsync(leitura.SensorId);

                if (sensor == null)
                {
                    _logger.LogWarning($"Sensor RFID não encontrado: {leitura.SensorId}");
                    return new ServiceResponse<LocalizacaoMotoDTO>
                    {
                        Success = false,
                        Message = $"Sensor RFID {leitura.SensorId} não encontrado"
                    };
                }

                if (sensor.ZonaPatio == null)
                {
                    _logger.LogWarning($"Sensor {leitura.SensorId} não está associado a nenhuma zona");
                    return new ServiceResponse<LocalizacaoMotoDTO>
                    {
                        Success = false,
                        Message = $"Sensor {leitura.SensorId} não está associado a nenhuma zona"
                    };
                }

                var (coordenadaFinalX, coordenadaFinalY) = CalcularCoordenadas(sensor, leitura.PotenciaSinal);
                var tipoEvento = DeterminarTipoEvento(sensor.ZonaPatio.TipoZona);
                var status = DeterminarStatusMoto(tipoEvento);

                EventoMoto? evento = new EventoMoto(
                    moto.Id,
                    tipoEvento,
                    null,
                    leitura.SensorId,
                    null,
                    null,
                    FonteDados.RFID
                );

                LocalizacaoMoto? localizacao = new LocalizacaoMoto(
                    moto.Id,
                    sensor.ZonaPatio.PatioId,
                    new Coordenada(coordenadaFinalX, coordenadaFinalY),
                    status,
                    FonteDados.RFID,
                    CalcularConfiabilidade(sensor)
                );

                try
                {
                    moto.AlterarStatus(status);
                    await _motoRepository.UpdateAsync(moto);
                }
                catch (Exception ex)
                {
                    _logger.LogWarning($"Não foi possível atualizar o status da moto: {ex.Message}");
                }

                await Task.WhenAll(
                    _eventoRepository.AddAsync(evento),
                    _localizacaoRepository.AddAsync(localizacao)
                );

                await _eventoRepository.SaveChangesAsync();

                //await NotificarAtualizacaoLocalizacao(localizacao);

                LocalizacaoMotoDTO localizacaoDto = new LocalizacaoMotoDTO
                {
                    Id = localizacao.Id,
                    MotoId = moto.Id,
                    PatioId = sensor.ZonaPatio.PatioId,
                    CoordenadaX = coordenadaFinalX,
                    CoordenadaY = coordenadaFinalY,
                    Timestamp = DateTime.UtcNow,
                    Status = status,
                    FonteDados = FonteDados.RFID,
                    Confiabilidade = CalcularConfiabilidade(sensor)
                };

                return new ServiceResponse<LocalizacaoMotoDTO>
                {
                    Success = true,
                    Message = "Leitura RFID processada com sucesso",
                    Data = localizacaoDto
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Erro ao processar leitura RFID {leitura.RFID}: {ex.Message}");
                return new ServiceResponse<LocalizacaoMotoDTO>
                {
                    Success = false,
                    Message = $"Erro ao processar leitura RFID: {ex.Message}",
                    Data = null
                };
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
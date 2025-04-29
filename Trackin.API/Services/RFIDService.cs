using Microsoft.EntityFrameworkCore;
using Trackin.API.Domain.Entity;
using Trackin.API.Domain.Enums;
using Trackin.API.DTOs;
using Trackin.API.Infrastructure.Context;

namespace Trackin.API.Services
{
    public class RFIDService
    {
        private readonly TrackinContext _context;
        private readonly ILogger<RFIDService> _logger;
        public RFIDService(TrackinContext context, ILogger<RFIDService> logger)
        {
            _context = context;
            _logger = logger;   
        }

        public async Task<ServiceResponse<LocalizacaoMotoDTO>> ProcessarLeituraRFID(RFIDLeituraDTO leitura)
        {
            try
            {
                Moto? moto = await _context.Motos.FirstOrDefaultAsync(m => m.RFIDTag == leitura.RFID);
                if (moto == null)
                {
                    _logger.LogWarning($"RFID não encontrado: {leitura.RFID}");
                    return new ServiceResponse<LocalizacaoMotoDTO>
                    {
                        Success = false,
                        Message = $"Moto com RFID {leitura.RFID} não encontrada",
                        Data = null
                    };
                }

                SensorRFID? sensor = await _context.SensoresRFID
                    .Include(s => s.ZonaPatio)
                    .ThenInclude(z => z.Patio)
                    .FirstOrDefaultAsync(s => s.Id == leitura.SensorId);

                if (sensor == null)
                {
                    _logger.LogWarning($"Sensor RFID não encontrado: {leitura.SensorId}");
                    return new ServiceResponse<LocalizacaoMotoDTO>
                    {
                        Success = false,
                        Message = $"Sensor RFID {leitura.SensorId} não encontrado",
                        Data = null
                    };
                }

                if (sensor.ZonaPatio == null)
                {
                    _logger.LogWarning($"Sensor {leitura.SensorId} não está associado a nenhuma zona");
                    return new ServiceResponse<LocalizacaoMotoDTO>
                    {
                        Success = false,
                        Message = $"Sensor {leitura.SensorId} não está associado a nenhuma zona",
                        Data = null
                    };
                }

                var (coordenadaFinalX, coordenadaFinalY) = CalcularCoordenadas(sensor, leitura.CoordenadaX, leitura.CoordenadaY);
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

                LocalizacaoMoto? localizacao = new LocalizacaoMoto
                {
                    MotoId = moto.Id,
                    PatioId = sensor.ZonaPatio.PatioId,
                    CoordenadaX = coordenadaFinalX,
                    CoordenadaY = coordenadaFinalY,
                    Timestamp = DateTime.UtcNow,
                    Status = status,
                    FonteDados = FonteDados.RFID,
                    Confiabilidade = CalcularConfiabilidade(sensor)
                };

                try
                {
                    moto.AlterarStatus(status); 
                    _context.Motos.Update(moto);
                }
                catch (Exception ex)
                {
                    _logger.LogWarning($"Não foi possível atualizar o status da moto: {ex.Message}");
                }

                await _context.Eventos.AddAsync(evento);
                await _context.Localizacoes.AddAsync(localizacao);
                await _context.SaveChangesAsync();

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

        private (double x, double y) CalcularCoordenadas(SensorRFID sensor, double offsetX, double offsetY)
        {
            double baseX = sensor.PosicaoX;
            double baseY = sensor.PosicaoY;

            // offset para ajuste fino da posição
            return (baseX + offsetX, baseY + offsetY);
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
    }
}

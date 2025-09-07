using Trackin.Application.Common;
using Trackin.Application.DTOs;
using Trackin.Application.Interfaces;
using Trackin.Domain.Entity;
using Trackin.Domain.Interfaces;
using Trackin.Domain.ValueObjects;

namespace Trackin.Application.Services
{
    public class SensorRFIDService : ISensorRFIDService
    {
        private readonly ISensorRFIDRepository _sensorRFIDRepository;

        public SensorRFIDService(ISensorRFIDRepository sensorRFIDRepository)
        {
            _sensorRFIDRepository = sensorRFIDRepository;
        }

        public async Task<ServiceResponsePaginado<SensorRFID>> GetAllSensoresRFIDPaginatedAsync(
            int pageNumber,
            int pageSize,
            string? ordering = null,
            bool descendingOrder = false)
        {
            try
            {
                (IEnumerable<SensorRFID> items, int totalCount) = await _sensorRFIDRepository.GetAllPaginatedAsync(
                    pageNumber, pageSize, ordering, descendingOrder);

                return new ServiceResponsePaginado<SensorRFID>(items, pageNumber, pageSize, totalCount);
            }
            catch (Exception ex)
            {
                return new ServiceResponsePaginado<SensorRFID>(new List<SensorRFID>(), pageNumber, pageSize, 0)
                {
                    Success = false,
                    Message = $"Erro ao obter sensores RFID paginados: {ex.Message}"
                };
            }
        }

        public async Task<ServiceResponse<IEnumerable<SensorRFID>>> GetAllSensoresRFIDAsync()
        {
            try
            {
                IEnumerable<SensorRFID> sensores = await _sensorRFIDRepository.GetAllAsync();
                if (sensores == null || !sensores.Any())
                {
                    return new ServiceResponse<IEnumerable<SensorRFID>>
                    {
                        Success = false,
                        Message = "Nenhum sensor RFID encontrado."
                    };
                }

                return new ServiceResponse<IEnumerable<SensorRFID>>
                {
                    Success = true,
                    Data = sensores
                };
            }
            catch (Exception ex)
            {
                return new ServiceResponse<IEnumerable<SensorRFID>>
                {
                    Success = false,
                    Message = $"Erro ao obter sensores RFID: {ex.Message}"
                };
            }
        }

        public async Task<ServiceResponse<SensorRFID>> GetSensorRFIDByIdAsync(long id)
        {
            try
            {
                SensorRFID? sensorRFID = await _sensorRFIDRepository.GetByIdAsync(id);
                if (sensorRFID == null)
                {
                    return new ServiceResponse<SensorRFID>
                    {
                        Success = false,
                        Message = $"Sensor RFID com ID {id} não encontrado."
                    };
                }

                return new ServiceResponse<SensorRFID>
                {
                    Success = true,
                    Data = sensorRFID
                };
            }
            catch (Exception ex)
            {
                return new ServiceResponse<SensorRFID>
                {
                    Success = false,
                    Message = $"Erro ao obter sensor RFID: {ex.Message}"
                };
            }
        }

        public async Task<ServiceResponse<SensorRFID>> CreateSensorRFIDAsync(CriarSensorRFIdDTO sensorRFIDDTO)
        {
            try
            {
                Coordenada coordenada = new Coordenada(sensorRFIDDTO.PosicaoX, sensorRFIDDTO.PosicaoY);

                SensorRFID sensor = new SensorRFID(
                    sensorRFIDDTO.ZonaPatioId,
                    sensorRFIDDTO.PatioId,
                    sensorRFIDDTO.Posicao,
                    coordenada,
                    sensorRFIDDTO.Altura,
                    sensorRFIDDTO.AnguloVisao
                );

                await _sensorRFIDRepository.AddAsync(sensor);
                await _sensorRFIDRepository.SaveChangesAsync();

                return new ServiceResponse<SensorRFID>
                {
                    Success = true,
                    Message = "Sensor RFID criado com sucesso.",
                    Data = sensor
                };
            }
            catch (ArgumentException ex)
            {
                return new ServiceResponse<SensorRFID>
                {
                    Success = false,
                    Message = $"Dados inválidos: {ex.Message}"
                };
            }
            catch (Exception ex)
            {
                return new ServiceResponse<SensorRFID>
                {
                    Success = false,
                    Message = $"Erro ao criar sensor RFID: {ex.Message}"
                };
            }
        }

        public async Task<ServiceResponse<SensorRFID>> UpdateSensorRFIDAsync(long id, CriarSensorRFIdDTO sensorRFIDDTO)
        {
            try
            {
                SensorRFID? sensorRFID = await _sensorRFIDRepository.GetByIdAsync(id);
                if (sensorRFID == null)
                {
                    return new ServiceResponse<SensorRFID>
                    {
                        Success = false,
                        Message = $"Sensor RFID com ID {id} não encontrado."
                    };
                }

                Coordenada novaCoordenada = new Coordenada(sensorRFIDDTO.PosicaoX, sensorRFIDDTO.PosicaoY);
                sensorRFID.AtualizarPosicao(novaCoordenada, sensorRFIDDTO.Altura);

                // criar metodos na entidade para atualizar outros campos 

                await _sensorRFIDRepository.SaveChangesAsync();

                return new ServiceResponse<SensorRFID>
                {
                    Success = true,
                    Message = "Sensor RFID atualizado com sucesso.",
                    Data = sensorRFID
                };
            }
            catch (ArgumentException ex)
            {
                return new ServiceResponse<SensorRFID>
                {
                    Success = false,
                    Message = $"Dados inválidos: {ex.Message}"
                };
            }
            catch (InvalidOperationException ex)
            {
                return new ServiceResponse<SensorRFID>
                {
                    Success = false,
                    Message = $"Operação inválida: {ex.Message}"
                };
            }
            catch (Exception ex)
            {
                return new ServiceResponse<SensorRFID>
                {
                    Success = false,
                    Message = $"Erro ao atualizar sensor RFID: {ex.Message}"
                };
            }
        }

        public async Task<ServiceResponse<SensorRFID>> DeleteSensorRFIDAsync(long id)
        {
            try
            {
                SensorRFID? sensorRFID = await _sensorRFIDRepository.GetByIdAsync(id);
                if (sensorRFID == null)
                {
                    return new ServiceResponse<SensorRFID>
                    {
                        Success = false,
                        Message = $"Sensor RFID com ID {id} não encontrado."
                    };
                }

                if (sensorRFID.Ativo)
                {
                    sensorRFID.DesativarSensor();
                }

                await _sensorRFIDRepository.RemoveAsync(sensorRFID);
                await _sensorRFIDRepository.SaveChangesAsync();

                return new ServiceResponse<SensorRFID>
                {
                    Success = true,
                    Message = "Sensor RFID removido com sucesso.",
                    Data = sensorRFID
                };
            }
            catch (InvalidOperationException ex)
            {
                return new ServiceResponse<SensorRFID>
                {
                    Success = false,
                    Message = $"Operação inválida: {ex.Message}"
                };
            }
            catch (Exception ex)
            {
                return new ServiceResponse<SensorRFID>
                {
                    Success = false,
                    Message = $"Erro ao remover sensor RFID: {ex.Message}"
                };
            }
        }

        public async Task<ServiceResponse<SensorRFID>> AtivarSensorAsync(long id)
        {
            try
            {
                SensorRFID sensorRFID = await _sensorRFIDRepository.GetByIdAsync(id);
                if (sensorRFID == null)
                {
                    return new ServiceResponse<SensorRFID>
                    {
                        Success = false,
                        Message = $"Sensor RFID com ID {id} não encontrado."
                    };
                }

                sensorRFID.AtivarSensor();
                await _sensorRFIDRepository.SaveChangesAsync();

                return new ServiceResponse<SensorRFID>
                {
                    Success = true,
                    Message = "Sensor RFID ativado com sucesso.",
                    Data = sensorRFID
                };
            }
            catch (InvalidOperationException ex)
            {
                return new ServiceResponse<SensorRFID>
                {
                    Success = false,
                    Message = ex.Message
                };
            }
            catch (Exception ex)
            {
                return new ServiceResponse<SensorRFID>
                {
                    Success = false,
                    Message = $"Erro ao ativar sensor RFID: {ex.Message}"
                };
            }
        }

        public async Task<ServiceResponse<SensorRFID>> DesativarSensorAsync(long id)
        {
            try
            {
                SensorRFID sensorRFID = await _sensorRFIDRepository.GetByIdAsync(id);
                if (sensorRFID == null)
                {
                    return new ServiceResponse<SensorRFID>
                    {
                        Success = false,
                        Message = $"Sensor RFID com ID {id} não encontrado."
                    };
                }

                sensorRFID.DesativarSensor();
                await _sensorRFIDRepository.SaveChangesAsync();

                return new ServiceResponse<SensorRFID>
                {
                    Success = true,
                    Message = "Sensor RFID desativado com sucesso.",
                    Data = sensorRFID
                };
            }
            catch (InvalidOperationException ex)
            {
                return new ServiceResponse<SensorRFID>
                {
                    Success = false,
                    Message = ex.Message
                };
            }
            catch (Exception ex)
            {
                return new ServiceResponse<SensorRFID>
                {
                    Success = false,
                    Message = $"Erro ao desativar sensor RFID: {ex.Message}"
                };
            }
        }

        public async Task<ServiceResponse<IEnumerable<SensorRFID>>> GetSensoresComProblemaAsync(TimeSpan tempoLimiteSemLeitura)
        {
            try
            {
                IEnumerable<SensorRFID> sensores = await _sensorRFIDRepository.GetAllAsync();
                IEnumerable<SensorRFID> sensoresComProblema = sensores.Where(s => s.EstaComProblema(tempoLimiteSemLeitura));

                return new ServiceResponse<IEnumerable<SensorRFID>>
                {
                    Success = true,
                    Data = sensoresComProblema
                };
            }
            catch (Exception ex)
            {
                return new ServiceResponse<IEnumerable<SensorRFID>>
                {
                    Success = false,
                    Message = $"Erro ao obter sensores com problema: {ex.Message}"
                };
            }
        }

        public async Task<ServiceResponse<bool>> VerificarSensorPodeSerUsadoAsync(long id)
        {
            try
            {
                SensorRFID sensorRFID = await _sensorRFIDRepository.GetByIdAsync(id);
                if (sensorRFID == null)
                {
                    return new ServiceResponse<bool>
                    {
                        Success = false,
                        Message = $"Sensor RFID com ID {id} não encontrado."
                    };
                }

                bool podeSerUsado = sensorRFID.PodeLerRFID();

                return new ServiceResponse<bool>
                {
                    Success = true,
                    Data = podeSerUsado,
                    Message = podeSerUsado ? "Sensor pode ser usado" : "Sensor não pode ser usado (inativo)"
                };
            }
            catch (Exception ex)
            {
                return new ServiceResponse<bool>
                {
                    Success = false,
                    Message = $"Erro ao verificar status do sensor: {ex.Message}"
                };
            }
        }

        private async Task<bool> SensorRFIDExistsAsync(long id)
        {
            try
            {
                SensorRFID sensor = await _sensorRFIDRepository.GetByIdAsync(id);
                return sensor != null;
            }
            catch
            {
                return false;
            }
        }
    }
}
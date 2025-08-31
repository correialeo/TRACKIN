using Microsoft.EntityFrameworkCore;
using Trackin.API.Common;
using Trackin.API.Domain.Entity;
using Trackin.API.DTOs;
using Trackin.API.Infrastructure.Persistence.Repositories;

namespace Trackin.API.Services
{
    public class SensorRFIDService
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
                SensorRFID sensor = new SensorRFID
                {
                    ZonaPatioId = sensorRFIDDTO.ZonaPatioId,
                    PatioId = sensorRFIDDTO.PatioId,
                    Posicao = sensorRFIDDTO.Posicao,
                    PosicaoX = sensorRFIDDTO.PosicaoX,
                    PosicaoY = sensorRFIDDTO.PosicaoY,
                    Altura = sensorRFIDDTO.Altura,
                    AnguloVisao = sensorRFIDDTO.AnguloVisao
                };

                await _sensorRFIDRepository.AddAsync(sensor);
                await _sensorRFIDRepository.SaveChangesAsync();

                return new ServiceResponse<SensorRFID>
                {
                    Success = true,
                    Message = "Sensor RFID criado com sucesso.",
                    Data = sensor
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

                sensorRFID.ZonaPatioId = sensorRFIDDTO.ZonaPatioId;
                sensorRFID.PatioId = sensorRFIDDTO.PatioId;
                sensorRFID.Posicao = sensorRFIDDTO.Posicao;
                sensorRFID.PosicaoX = sensorRFIDDTO.PosicaoX;
                sensorRFID.PosicaoY = sensorRFIDDTO.PosicaoY;
                sensorRFID.Altura = sensorRFIDDTO.Altura;
                sensorRFID.AnguloVisao = sensorRFIDDTO.AnguloVisao;

                await _sensorRFIDRepository.SaveChangesAsync();

                return new ServiceResponse<SensorRFID>
                {
                    Success = true,
                    Message = "Sensor RFID atualizado com sucesso.",
                    Data = sensorRFID
                };
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await SensorRFIDExistsAsync(id))
                {
                    return new ServiceResponse<SensorRFID>
                    {
                        Success = false,
                        Message = $"Sensor RFID com ID {id} não encontrado."
                    };
                }
                else
                {
                    throw;
                }
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

                await _sensorRFIDRepository.RemoveAsync(sensorRFID);
                await _sensorRFIDRepository.SaveChangesAsync();

                return new ServiceResponse<SensorRFID>
                {
                    Success = true,
                    Message = "Sensor RFID removido com sucesso.",
                    Data = sensorRFID
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

        private async Task<bool> SensorRFIDExistsAsync(long id)
        {
            try
            {
                IEnumerable<SensorRFID> sensores = await _sensorRFIDRepository.GetAllAsync();
                return sensores.Any(e => e.Id == id);
            }
            catch
            {
                return false;
            }
        }
    }
}


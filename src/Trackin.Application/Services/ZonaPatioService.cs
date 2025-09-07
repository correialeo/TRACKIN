
using Trackin.Application.Common;
using Trackin.Application.DTOs;
using Trackin.Application.Interfaces;
using Trackin.Domain.Entity;
using Trackin.Domain.Interfaces;

namespace Trackin.Application.Services
{
    public class ZonaPatioService : IZonaPatioService
    {
        private readonly IZonaPatioRepository _zonaPatioRepository;

        public ZonaPatioService(IZonaPatioRepository zonaPatioRepository)
        {
            _zonaPatioRepository = zonaPatioRepository;
        }

        public async Task<ServiceResponsePaginado<ZonaPatio>> GetAllZonasPatiosPaginatedAsync(
            int pageNumber,
            int pageSize,
            string? ordering = null,
            bool descendingOrder = false)
        {
            try
            {
                (IEnumerable<ZonaPatio> items, int totalCount) = await _zonaPatioRepository.GetAllPaginatedAsync(
                    pageNumber, pageSize, ordering, descendingOrder);

                return new ServiceResponsePaginado<ZonaPatio>(items, pageNumber, pageSize, totalCount);
            }
            catch (Exception ex)
            {
                return new ServiceResponsePaginado<ZonaPatio>(new List<ZonaPatio>(), pageNumber, pageSize, 0)
                {
                    Success = false,
                    Message = $"Erro ao obter zonas de pátio paginadas: {ex.Message}"
                };
            }
        }

        public async Task<ServiceResponse<IEnumerable<ZonaPatio>>> GetAllZonasPatiosAsync()
        {
            try
            {
                IEnumerable<ZonaPatio> zonasPatio = await _zonaPatioRepository.GetAllAsync();
                if (zonasPatio == null || !zonasPatio.Any())
                {
                    return new ServiceResponse<IEnumerable<ZonaPatio>>
                    {
                        Success = false,
                        Message = "Nenhuma zona de pátio encontrada."
                    };
                }

                return new ServiceResponse<IEnumerable<ZonaPatio>>
                {
                    Success = true,
                    Data = zonasPatio
                };
            }
            catch (Exception ex)
            {
                return new ServiceResponse<IEnumerable<ZonaPatio>>
                {
                    Success = false,
                    Message = $"Erro ao obter zonas de pátio: {ex.Message}"
                };
            }
        }

        public async Task<ServiceResponse<ZonaPatio>> GetZonaPatioByIdAsync(long id)
        {
            try
            {
                ZonaPatio? zonaPatio = await _zonaPatioRepository.GetByIdAsync(id);
                if (zonaPatio == null)
                {
                    return new ServiceResponse<ZonaPatio>
                    {
                        Success = false,
                        Message = $"Zona de pátio com ID {id} não encontrada."
                    };
                }

                return new ServiceResponse<ZonaPatio>
                {
                    Success = true,
                    Data = zonaPatio
                };
            }
            catch (Exception ex)
            {
                return new ServiceResponse<ZonaPatio>
                {
                    Success = false,
                    Message = $"Erro ao obter zona de pátio: {ex.Message}"
                };
            }
        }

        public async Task<ServiceResponse<ZonaPatio>> CreateZonaPatioAsync(CriarZonaPatioDTO dto)
        {
            try
            {
                ZonaPatio zonaPatio = new ZonaPatio
                {
                    PatioId = dto.PatioId,
                    Nome = dto.Nome,
                    TipoZona = dto.TipoZona,
                    CoordenadaInicialX = dto.CoordenadaInicialX,
                    CoordenadaInicialY = dto.CoordenadaInicialY,
                    CoordenadaFinalX = dto.CoordenadaFinalX,
                    CoordenadaFinalY = dto.CoordenadaFinalY,
                    Cor = dto.Cor
                };

                await _zonaPatioRepository.AddAsync(zonaPatio);
                await _zonaPatioRepository.SaveChangesAsync();

                return new ServiceResponse<ZonaPatio>
                {
                    Success = true,
                    Message = "Zona de pátio criada com sucesso.",
                    Data = zonaPatio
                };
            }
            catch (Exception ex)
            {
                return new ServiceResponse<ZonaPatio>
                {
                    Success = false,
                    Message = $"Erro ao criar zona de pátio: {ex.Message}"
                };
            }
        }

        public async Task<ServiceResponse<ZonaPatio>> UpdateZonaPatioAsync(long id, CriarZonaPatioDTO zonaPatioDto)
        {
            try
            {
                ZonaPatio? zonaPatio = await _zonaPatioRepository.GetByIdAsync(id);
                if (zonaPatio == null)
                {
                    return new ServiceResponse<ZonaPatio>
                    {
                        Success = false,
                        Message = $"Zona de pátio com ID {id} não encontrada."
                    };
                }

                zonaPatio.PatioId = zonaPatioDto.PatioId;
                zonaPatio.Nome = zonaPatioDto.Nome;
                zonaPatio.TipoZona = zonaPatioDto.TipoZona;
                zonaPatio.CoordenadaInicialX = zonaPatioDto.CoordenadaInicialX;
                zonaPatio.CoordenadaInicialY = zonaPatioDto.CoordenadaInicialY;
                zonaPatio.CoordenadaFinalX = zonaPatioDto.CoordenadaFinalX;
                zonaPatio.CoordenadaFinalY = zonaPatioDto.CoordenadaFinalY;
                zonaPatio.Cor = zonaPatioDto.Cor;

                await _zonaPatioRepository.SaveChangesAsync();

                return new ServiceResponse<ZonaPatio>
                {
                    Success = true,
                    Message = "Zona de pátio atualizada com sucesso.",
                    Data = zonaPatio
                };
            }
            catch (Exception ex)
            {
                return new ServiceResponse<ZonaPatio>
                {
                    Success = false,
                    Message = $"Erro ao atualizar zona de pátio: {ex.Message}"
                };
            }
        }

        public async Task<ServiceResponse<ZonaPatio>> DeleteZonaPatioAsync(long id)
        {
            try
            {
                ZonaPatio? zonaPatio = await _zonaPatioRepository.GetByIdAsync(id);
                if (zonaPatio == null)
                {
                    return new ServiceResponse<ZonaPatio>
                    {
                        Success = false,
                        Message = $"Zona de pátio com ID {id} não encontrada."
                    };
                }

                await _zonaPatioRepository.RemoveAsync(zonaPatio);
                await _zonaPatioRepository.SaveChangesAsync();

                return new ServiceResponse<ZonaPatio>
                {
                    Success = true,
                    Message = "Zona de pátio removida com sucesso.",
                    Data = zonaPatio
                };
            }
            catch (Exception ex)
            {
                return new ServiceResponse<ZonaPatio>
                {
                    Success = false,
                    Message = $"Erro ao remover zona de pátio: {ex.Message}"
                };
            }
        }

        private async Task<bool> ZonaPatioExistsAsync(long id)
        {
            try
            {
                IEnumerable<ZonaPatio> zonas = await _zonaPatioRepository.GetAllAsync();
                return zonas.Any(e => e.Id == id);
            }
            catch
            {
                return false;
            }
        }
    }
}


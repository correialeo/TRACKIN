using Trackin.API.Common;
using Trackin.API.Domain.Entity;
using Trackin.API.DTOs;
using Trackin.API.Infrastructure.Persistence.Repositories;

namespace Trackin.API.Services
{
    public class PatioService
    {
        private readonly IPatioRepository _patioRepository;

        public PatioService(IPatioRepository patioRepository)
        {
            _patioRepository = patioRepository;
        }

        public async Task<ServiceResponsePaginado<Patio>> GetAllPatiosPaginatedAsync(
            int pageNumber,
            int pageSize,
            string? ordering = null,
            bool descendingOrder = false)
        {
            try
            {
                (IEnumerable<Patio> items, int totalCount) = await _patioRepository.GetAllPaginatedAsync(
                    pageNumber, pageSize, ordering, descendingOrder);

                return new ServiceResponsePaginado<Patio>(items, pageNumber, pageSize, totalCount);
            }
            catch (Exception ex)
            {
                return new ServiceResponsePaginado<Patio>(new List<Patio>(), pageNumber, pageSize, 0)
                {
                    Success = false,
                    Message = $"Erro ao obter pátios paginados: {ex.Message}"
                };
            }
        }

        public async Task<ServiceResponse<IEnumerable<Patio>>> GetAllPatiosAsync()
        {
            try
            {
                IEnumerable<Patio> patios = await _patioRepository.GetAllAsync();
                if (patios == null || !patios.Any())
                {
                    return new ServiceResponse<IEnumerable<Patio>>
                    {
                        Success = false,
                        Message = "Nenhum pátio encontrado."
                    };
                }

                return new ServiceResponse<IEnumerable<Patio>>
                {
                    Success = true,
                    Data = patios
                };
            }
            catch (Exception ex)
            {
                return new ServiceResponse<IEnumerable<Patio>>
                {
                    Success = false,
                    Message = $"Erro ao obter pátios: {ex.Message}"
                };
            }
        }

        public async Task<ServiceResponse<Patio>> GetPatioByIdAsync(long id)
        {
            try
            {
                Patio? patio = await _patioRepository.GetByIdAsync(id);
                if (patio == null)
                {
                    return new ServiceResponse<Patio>
                    {
                        Success = false,
                        Message = $"Pátio com ID {id} não encontrado."
                    };
                }

                return new ServiceResponse<Patio>
                {
                    Success = true,
                    Data = patio
                };
            }
            catch (Exception ex)
            {
                return new ServiceResponse<Patio>
                {
                    Success = false,
                    Message = $"Erro ao obter pátio: {ex.Message}"
                };
            }
        }

        public async Task<ServiceResponse<Patio>> CreatePatioAsync(CriarPatioDto dto)
        {
            try
            {
                Patio patio = new Patio
                {
                    Nome = dto.Nome,
                    Endereco = dto.Endereco,
                    Cidade = dto.Cidade,
                    Estado = dto.Estado,
                    Pais = dto.Pais,
                    DimensaoX = dto.DimensaoX,
                    DimensaoY = dto.DimensaoY,
                    PlantaBaixa = dto.PlantaBaixa,
                };

                await _patioRepository.AddAsync(patio);
                await _patioRepository.SaveChangesAsync();

                return new ServiceResponse<Patio>
                {
                    Success = true,
                    Message = "Pátio criado com sucesso.",
                    Data = patio
                };
            }
            catch (Exception ex)
            {
                return new ServiceResponse<Patio>
                {
                    Success = false,
                    Message = $"Erro ao criar pátio: {ex.Message}"
                };
            }
        }

        public async Task<ServiceResponse<Patio>> DeletePatioAsync(long id)
        {
            try
            {
                Patio? patio = await _patioRepository.GetByIdAsync(id);
                if (patio == null)
                {
                    return new ServiceResponse<Patio>
                    {
                        Success = false,
                        Message = $"Pátio com ID {id} não encontrado."
                    };
                }

                await _patioRepository.RemoveAsync(patio);
                await _patioRepository.SaveChangesAsync();

                return new ServiceResponse<Patio>
                {
                    Success = true,
                    Message = "Pátio removido com sucesso.",
                    Data = patio
                };
            }
            catch (Exception ex)
            {
                return new ServiceResponse<Patio>
                {
                    Success = false,
                    Message = $"Erro ao remover pátio: {ex.Message}"
                };
            }
        }
    }
}


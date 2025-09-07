using Trackin.Application.Common;
using Trackin.Application.DTOs;
using Trackin.Application.Interfaces;
using Trackin.Domain.Entity;
using Trackin.Domain.Interfaces;
using Trackin.Domain.ValueObjects;

namespace Trackin.Application.Services
{
    public class PatioService : IPatioService
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
                Patio patio = new Patio(
                    nome: dto.Nome,
                    endereco: dto.Endereco,
                    cidade: dto.Cidade,
                    estado: dto.Estado,
                    pais: dto.Pais,
                    largura: dto.DimensaoX,
                    comprimento: dto.DimensaoY
                );

                await _patioRepository.AddAsync(patio);
                await _patioRepository.SaveChangesAsync();

                return new ServiceResponse<Patio>
                {
                    Success = true,
                    Message = "Pátio criado com sucesso.",
                    Data = patio
                };
            }
            catch (ArgumentException ex)
            {
                return new ServiceResponse<Patio>
                {
                    Success = false,
                    Message = $"Dados inválidos: {ex.Message}"
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

                if (patio.Cameras.Any() || patio.Zonas.Any() || patio.SensoresRFID.Any())
                {
                    return new ServiceResponse<Patio>
                    {
                        Success = false,
                        Message = "Não é possível remover um pátio que possui câmeras, zonas ou sensores associados."
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

        public async Task<ServiceResponse<double>> GetTaxaOcupacaoAsync(long patioId)
        {
            try
            {
                Patio patio = await _patioRepository.GetByIdAsync(patioId);
                if (patio == null)
                {
                    return new ServiceResponse<double>
                    {
                        Success = false,
                        Message = $"Pátio com ID {patioId} não encontrado."
                    };
                }

                double taxaOcupacao = patio.CalcularTaxaOcupacao();

                return new ServiceResponse<double>
                {
                    Success = true,
                    Data = taxaOcupacao
                };
            }
            catch (Exception ex)
            {
                return new ServiceResponse<double>
                {
                    Success = false,
                    Message = $"Erro ao calcular taxa de ocupação: {ex.Message}"
                };
            }
        }

        public async Task<ServiceResponse<bool>> ValidarCoordenadaAsync(long patioId, double x, double y)
        {
            try
            {
                Patio patio = await _patioRepository.GetByIdAsync(patioId);
                if (patio == null)
                {
                    return new ServiceResponse<bool>
                    {
                        Success = false,
                        Message = $"Pátio com ID {patioId} não encontrado."
                    };
                }

                Coordenada coordenada = new Coordenada(x, y);
                bool coordenadaValida = patio.CoordenadaEstaValida(coordenada);

                return new ServiceResponse<bool>
                {
                    Success = true,
                    Data = coordenadaValida
                };
            }
            catch (Exception ex)
            {
                return new ServiceResponse<bool>
                {
                    Success = false,
                    Message = $"Erro ao validar coordenada: {ex.Message}"
                };
            }
        }
    }
}
using Trackin.Application.Common;
using Trackin.Application.DTOs;
using Trackin.Application.Interfaces;
using Trackin.Domain.Entity;
using Trackin.Domain.Interfaces;
using Trackin.Domain.ValueObjects;

namespace Trackin.Application.Services
{
    public class ZonaPatioService : IZonaPatioService
    {
        private readonly IZonaPatioRepository _zonaPatioRepository;
        private readonly IPatioRepository _patioRepository;

        public ZonaPatioService(IZonaPatioRepository zonaPatioRepository, IPatioRepository patioRepository)
        {
            _zonaPatioRepository = zonaPatioRepository;
            _patioRepository = patioRepository;
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
                Patio patio = await _patioRepository.GetByIdAsync(dto.PatioId);
                if (patio == null)
                {
                    return new ServiceResponse<ZonaPatio>
                    {
                        Success = false,
                        Message = $"Pátio com ID {dto.PatioId} não encontrado."
                    };
                }

                Coordenada pontoInicial = new Coordenada(dto.CoordenadaInicialX, dto.CoordenadaInicialY);
                Coordenada pontoFinal = new Coordenada(dto.CoordenadaFinalX, dto.CoordenadaFinalY);

                ZonaPatio zonaPatio = patio.CriarZona(
                    nome: dto.Nome,
                    tipoZona: dto.TipoZona,
                    pontoInicial: pontoInicial,
                    pontoFinal: pontoFinal,
                    cor: dto.Cor
                );

                await _patioRepository.SaveChangesAsync();

                return new ServiceResponse<ZonaPatio>
                {
                    Success = true,
                    Message = "Zona de pátio criada com sucesso.",
                    Data = zonaPatio
                };
            }
            catch (ArgumentException ex)
            {
                return new ServiceResponse<ZonaPatio>
                {
                    Success = false,
                    Message = $"Dados inválidos: {ex.Message}"
                };
            }
            catch (InvalidOperationException ex)
            {
                return new ServiceResponse<ZonaPatio>
                {
                    Success = false,
                    Message = $"Operação inválida: {ex.Message}"
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

                if (!string.IsNullOrEmpty(zonaPatioDto.Cor) && zonaPatioDto.Cor != zonaPatio.Cor)
                {
                    zonaPatio.AlterarCor(zonaPatioDto.Cor);
                }

                Coordenada novoPontoInicial = new Coordenada(zonaPatioDto.CoordenadaInicialX, zonaPatioDto.CoordenadaInicialY);
                Coordenada novoPontoFinal = new Coordenada(zonaPatioDto.CoordenadaFinalX, zonaPatioDto.CoordenadaFinalY);

                Patio patio = await _patioRepository.GetByIdAsync(zonaPatio.PatioId);
                if (patio == null)
                {
                    return new ServiceResponse<ZonaPatio>
                    {
                        Success = false,
                        Message = "Pátio associado não encontrado."
                    };
                }

                if (!patio.CoordenadaEstaValida(novoPontoInicial) || !patio.CoordenadaEstaValida(novoPontoFinal))
                {
                    return new ServiceResponse<ZonaPatio>
                    {
                        Success = false,
                        Message = "As coordenadas especificadas estão fora dos limites do pátio."
                    };
                }

                zonaPatio.RedimensionarZona(novoPontoInicial, novoPontoFinal);

                await _zonaPatioRepository.SaveChangesAsync();

                return new ServiceResponse<ZonaPatio>
                {
                    Success = true,
                    Message = "Zona de pátio atualizada com sucesso.",
                    Data = zonaPatio
                };
            }
            catch (ArgumentException ex)
            {
                return new ServiceResponse<ZonaPatio>
                {
                    Success = false,
                    Message = $"Dados inválidos: {ex.Message}"
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

                if (zonaPatio.SensoresRFID.Any())
                {
                    return new ServiceResponse<ZonaPatio>
                    {
                        Success = false,
                        Message = "Não é possível remover uma zona que possui sensores RFID associados."
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

        public async Task<ServiceResponse<double>> CalcularAreaZonaAsync(long zonaId)
        {
            try
            {
                ZonaPatio zona = await _zonaPatioRepository.GetByIdAsync(zonaId);
                if (zona == null)
                {
                    return new ServiceResponse<double>
                    {
                        Success = false,
                        Message = $"Zona com ID {zonaId} não encontrada."
                    };
                }

                double area = zona.CalcularArea();

                return new ServiceResponse<double>
                {
                    Success = true,
                    Data = area
                };
            }
            catch (Exception ex)
            {
                return new ServiceResponse<double>
                {
                    Success = false,
                    Message = $"Erro ao calcular área da zona: {ex.Message}"
                };
            }
        }

        public async Task<ServiceResponse<Coordenada>> ObterCentroZonaAsync(long zonaId)
        {
            try
            {
                ZonaPatio zona = await _zonaPatioRepository.GetByIdAsync(zonaId);
                if (zona == null)
                {
                    return new ServiceResponse<Coordenada>
                    {
                        Success = false,
                        Message = $"Zona com ID {zonaId} não encontrada."
                    };
                }

                Coordenada centro = zona.ObterCentroZona();

                return new ServiceResponse<Coordenada>
                {
                    Success = true,
                    Data = centro
                };
            }
            catch (Exception ex)
            {
                return new ServiceResponse<Coordenada>
                {
                    Success = false,
                    Message = $"Erro ao obter centro da zona: {ex.Message}"
                };
            }
        }

        public async Task<ServiceResponse<bool>> VerificarPosicaoNaZonaAsync(long zonaId, double x, double y)
        {
            try
            {
                ZonaPatio zona = await _zonaPatioRepository.GetByIdAsync(zonaId);
                if (zona == null)
                {
                    return new ServiceResponse<bool>
                    {
                        Success = false,
                        Message = $"Zona com ID {zonaId} não encontrada."
                    };
                }

                Coordenada posicao = new Coordenada(x, y);
                bool contemPosicao = zona.ContemPosicao(posicao);

                return new ServiceResponse<bool>
                {
                    Success = true,
                    Data = contemPosicao
                };
            }
            catch (Exception ex)
            {
                return new ServiceResponse<bool>
                {
                    Success = false,
                    Message = $"Erro ao verificar posição na zona: {ex.Message}"
                };
            }
        }

        private async Task<bool> ZonaPatioExistsAsync(long id)
        {
            try
            {
                ZonaPatio zona = await _zonaPatioRepository.GetByIdAsync(id);
                return zona != null;
            }
            catch
            {
                return false;
            }
        }
    }
}
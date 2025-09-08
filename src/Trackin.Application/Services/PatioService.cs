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

        private const string PatioNaoExiste = "Não existe pátio cadastrado com id informado.";

        public PatioService(IPatioRepository patioRepository)
        {
            _patioRepository = patioRepository;
        }
        
        private async Task<Patio?> ObterPatio(long id) => await _patioRepository.GetByIdAsync(id);

        private ServiceResponse<T> Sucesso<T>(T data, string message = "") => new() { Success = true, Data = data, Message = message };

        private ServiceResponse<T> Erro<T>(string message) => new() { Success = false, Message = message };

        private (bool podeRemover, string? mensagem) PodeRemoverPatio(Patio patio)
        {
            if (patio.Cameras.Any() || patio.Zonas.Any() || patio.SensoresRFID.Any())
                return (false, "Não é possível remover um pátio que possui câmeras, zonas ou sensores associados.");
            return (true, null);
        }

        public async Task<ServiceResponsePaginado<Patio>> GetAllPatiosPaginatedAsync(
            int pageNumber,
            int pageSize,
            string? ordering = null,
            bool descendingOrder = false)
        {
            try
            {
                var( items, totalCount) = await _patioRepository.GetAllPaginatedAsync(
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
              var patios = await _patioRepository.GetAllAsync();
              if (!patios.Any())
                  return Erro<IEnumerable<Patio>>("Nenhum pátio encontrado");

              return Sucesso(patios);
            }
            catch (Exception ex)
            {
                return Erro<IEnumerable<Patio>>( $"Erro ao obter pátios: {ex.Message}");
            }
        }

        public async Task<ServiceResponse<Patio>> GetPatioByIdAsync(long id)
        {
            try
            {
                var patio = await ObterPatio(id);
                if (patio == null) 
                    return Erro<Patio>(PatioNaoExiste);
                
                return Sucesso(patio);
            }
            catch (Exception ex)
            {
                return Erro<Patio>($"Erro ao obter pátio:{ex.Message}");
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

                return Sucesso(patio, "Pátio criado com sucesso.");
            }
            catch (ArgumentException ex)
            {
                return Erro<Patio>($"dados inválidos: {ex.Message}");
            }
            catch (Exception ex)
            {
                return Erro<Patio>($"Erro ao criar pátio: {ex.Message}");
            }
        }

        public async Task<ServiceResponse<Patio>> DeletePatioAsync(long id)
        {
            try
            {
                var patio = await ObterPatio(id);
                if (patio == null) return Erro<Patio>(PatioNaoExiste);

                var (podeRemover, mensagem) = PodeRemoverPatio(patio);
                if (!podeRemover) return Erro<Patio>(mensagem!);

                await _patioRepository.RemoveAsync(patio);
                await _patioRepository.SaveChangesAsync();

                return Sucesso(patio, "Pátio removido com sucesso");
            }
            catch (Exception ex)
            {
                return Erro<Patio>($"Erro ao remover pátio: {ex.Message}");
            }
        }
        
    }
}
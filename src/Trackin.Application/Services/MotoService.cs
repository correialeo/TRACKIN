

using Trackin.Application.Common;
using Trackin.Application.DTOs;
using Trackin.Application.Interfaces;
using Trackin.Domain.Entity;
using Trackin.Domain.Enums;
using Trackin.Domain.Interfaces;

namespace Trackin.Application.Services
{
    public class MotoService : IMotoService
    {
        private readonly IMotoRepository _motoRepository;
        private readonly IPatioRepository _patioRepository;
        
        private const string PatioNaoExiste = "Não existe umpátio cadastrado com id informado";
        public MotoService(IMotoRepository motoRepository, IPatioRepository patioRepository)
        {
            _motoRepository = motoRepository;
            _patioRepository = patioRepository;
        }
        
        private async Task<Moto?> ObterMoto(long id) => await _motoRepository.GetByIdAsync(id);

        private async Task<Patio?> ObterPatio(long id) => await _patioRepository.GetByIdAsync(id);

        private ServiceResponse<T> Sucesso<T>(T data, string message = "") => new() { Success = true, Data = data, Message = message };

        private ServiceResponse<T> Erro<T>(string message) => new() { Success = false, Message = message };


        public async Task<ServiceResponse<MotoDTO>> CreateMotoAsync(MotoDTO motoDTO)
        {
            try
            {
                var patio = await ObterPatio(motoDTO.PatioId);
                if(patio == null)
                    return Erro<MotoDTO>(PatioNaoExiste);

                Moto moto = new(motoDTO.PatioId, motoDTO.Placa, motoDTO.Modelo, motoDTO.Ano, motoDTO.RFIDTag);

                await _motoRepository.AddAsync(moto);
                await _motoRepository.SaveChangesAsync();

                motoDTO.Id = moto.Id;
                return Sucesso(motoDTO, "moto cadastrada com sucesso");
            }
            catch (Exception ex)
            {
                return Erro<MotoDTO>($"Erro ao cadastrar moto: {ex.Message}");
            }
        }

        public async Task<ServiceResponse<Moto>> GetMotoByIdAsync(long id)
        {
            try
            {
                var moto = await ObterMoto(id);
                if (moto == null)
                    return Erro<Moto>("Moto não encontrada");
                return Sucesso(moto);
            }
            catch (Exception ex)
            {
                return Erro<Moto>($"Erro ao obter moto: {ex.Message}");
            }
        }

        public async Task<ServiceResponse<IEnumerable<Moto>>> GetAllMotosAsync()
        {
            try
            {
               var motos = await _motoRepository.GetAllAsync();
                return Sucesso(motos);
            }
            catch (Exception ex)
            {
                return Erro<IEnumerable<Moto>>($"Erro ao obter motos: {ex.Message}");
            }
        }

        public async Task<ServiceResponsePaginado<Moto>> GetAllMotosPaginatedAsync(
            int pageNumber,
            int pageSize,
            string? ordering = null,
            bool descendingOrder = false)
        {
            try
            {
                (IEnumerable<Moto> items, int totalCount) = await _motoRepository.GetAllPaginatedAsync(
                    pageNumber, pageSize, ordering, descendingOrder);

                return new ServiceResponsePaginado<Moto>(items, pageNumber, pageSize, totalCount);
            }
            catch (Exception ex)
            {
                return new ServiceResponsePaginado<Moto>(new List<Moto>(), pageNumber, pageSize, 0)
                {
                    Success = false,
                    Message = $"Erro ao obter motos paginadas: {ex.Message}"
                };
            }
        }

        public async Task<ServiceResponsePaginado<Moto>> GetMotosByPatioPaginatedAsync(
            long patioId,
            int pageNumber,
            int pageSize,
            string? ordering = null,
            bool descendingOrder = false)
        {
            try
            {
                IEnumerable<Moto> allMotosInPatio = await _motoRepository.FindAsync(m => m.PatioId == patioId);

                IQueryable<Moto> query = allMotosInPatio.AsQueryable();
                if (!string.IsNullOrEmpty(ordering))
                {
                    query = ordering.ToLower() switch
                    {
                        "placa" => descendingOrder ? query.OrderByDescending(m => m.Placa) : query.OrderBy(m => m.Placa),
                        "modelo" => descendingOrder ? query.OrderByDescending(m => m.Modelo) : query.OrderBy(m => m.Modelo),
                        "ano" => descendingOrder ? query.OrderByDescending(m => m.Ano) : query.OrderBy(m => m.Ano),
                        _ => query.OrderBy(m => m.Id)
                    };
                }

                int totalCount = query.Count();
                List<Moto> items = query.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();

                return new ServiceResponsePaginado<Moto>(items, pageNumber, pageSize, totalCount);
            }
            catch (Exception ex)
            {
                return new ServiceResponsePaginado<Moto>(new List<Moto>(), pageNumber, pageSize, 0)
                {
                    Success = false,
                    Message = $"Erro ao obter motos por filial paginadas: {ex.Message}"
                };
            }
        }


        public async Task<ServiceResponsePaginado<Moto>> GetMotosByStatusPaginatedAsync(
            MotoStatus status,
            int pageNumber,
            int pageSize,
            string? ordering = null,
            bool descendingOrder = false)
        {
            try
            {
                IEnumerable<Moto> allMotosWithStatus = await _motoRepository.FindAsync(m => m.Status == status);

                IQueryable<Moto> query = allMotosWithStatus.AsQueryable();
                if (!string.IsNullOrEmpty(ordering))
                {
                    query = ordering.ToLower() switch
                    {
                        "placa" => descendingOrder ? query.OrderByDescending(m => m.Placa) : query.OrderBy(m => m.Placa),
                        "modelo" => descendingOrder ? query.OrderByDescending(m => m.Modelo) : query.OrderBy(m => m.Modelo),
                        "ano" => descendingOrder ? query.OrderByDescending(m => m.Ano) : query.OrderBy(m => m.Ano),
                        _ => query.OrderBy(m => m.Id)
                    };
                }

                int totalCount = query.Count();
                List<Moto> items = query.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();

                return new ServiceResponsePaginado<Moto>(items, pageNumber, pageSize, totalCount);
            }
            catch (Exception ex)
            {
                return new ServiceResponsePaginado<Moto>(new List<Moto>(), pageNumber, pageSize, 0)
                {
                    Success = false,
                    Message = $"Erro ao obter motos por status: {ex.Message}"
                };
            }
        }

        public async Task<ServiceResponse<Moto>> UpdateMotoAsync(long id)
        {
            try
            {
               var moto = await ObterMoto(id);
               if (moto == null)
                   return Erro<Moto>("moto não encontrada");

                await _motoRepository.UpdateMotoAsync(moto);
                await _motoRepository.SaveChangesAsync();
                
                return Sucesso(moto);
            }
            catch (Exception ex)
            {
               return Erro<Moto>($"Erro ao atualizar moto: {ex.Message}");
            }
        }

        public async Task<ServiceResponse<Moto>> DeleteMotoAsync(long id)
        {
            try
            {
                var moto = await ObterMoto(id);
                if (moto == null)
                    return Erro<Moto>("moto não encontrada");

                await _motoRepository.RemoveAsync(moto);
                await _motoRepository.SaveChangesAsync();

                return Sucesso(moto);
            }
            catch (Exception ex)
            {
               return Erro<Moto>($"Erro ao deletar moto: {ex.Message}");
            }
        }

        
    }
}


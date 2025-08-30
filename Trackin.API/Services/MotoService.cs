
using Microsoft.EntityFrameworkCore;
using Trackin.API.Common;
using Trackin.API.Domain.Entity;
using Trackin.API.Domain.Enums;
using Trackin.API.DTOs;
using Trackin.API.Infrastructure.Context;
using Trackin.API.Infrastructure.Persistence.Repositories;

namespace Trackin.API.Services
{
    public class MotoService
    {
        private readonly IMotoRepository _motoRepository;
        public MotoService(IMotoRepository motoRepository)
        {
            _motoRepository = motoRepository;
        }

        public async Task<ServiceResponse<MotoDTO>> CreateMotoAsync(MotoDTO motoDTO)
        {
            try
            {
                Moto moto = new(motoDTO.PatioId, motoDTO.Placa, motoDTO.Modelo, motoDTO.Ano, motoDTO.RFIDTag);

                await _motoRepository.AddAsync(moto);
                await _motoRepository.SaveChangesAsync();

                motoDTO.Id = moto.Id;
                return new ServiceResponse<MotoDTO>
                {
                    Success = true,
                    Message = "Moto cadastrada com sucesso.",
                    Data = motoDTO
                };
            }
            catch (Exception ex)
            {
                return new ServiceResponse<MotoDTO>
                {
                    Success = false,
                    Message = $"Erro ao cadastrar moto: {ex.Message}"
                };
            }
        }

        public async Task<ServiceResponse<Moto>> GetMotoByIdAsync(long id)
        {
            try
            {
                Moto? moto = await _motoRepository.GetByIdAsync(id);
                if (moto == null)
                {
                    return new ServiceResponse<Moto>
                    {
                        Success = false,
                        Message = "Moto não encontrada."
                    };
                }
                return new ServiceResponse<Moto>
                {
                    Success = true,
                    Data = moto
                };
            }
            catch (Exception ex)
            {
                return new ServiceResponse<Moto>
                {
                    Success = false,
                    Message = $"Erro ao obter moto: {ex.Message}"
                };
            }
        }

        public async Task<ServiceResponse<IEnumerable<Moto>>> GetAllMotosAsync()
        {
            try
            {
                IEnumerable<Moto> motos = await _motoRepository.GetAllAsync();
                return new ServiceResponse<IEnumerable<Moto>>
                {
                    Success = true,
                    Data = motos
                };
            }
            catch (Exception ex)
            {
                return new ServiceResponse<IEnumerable<Moto>>
                {
                    Success = false,
                    Message = $"Erro ao obter motos: {ex.Message}"
                };
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

        public async Task<ServiceResponse<Moto>> UpdateMotoAsync(long id, EditarMotoDTO motoDTO)
        {
            try
            {
                Moto? moto = await _motoRepository.GetByIdAsync(id);
                if (moto == null)
                {
                    return new ServiceResponse<Moto>
                    {
                        Success = false,
                        Message = "Moto não encontrada."
                    };
                }

                await _motoRepository.UpdateMotoAsync(moto, motoDTO);

                await _motoRepository.SaveChangesAsync();
                return new ServiceResponse<Moto>
                {
                    Success = true,
                    Data = moto
                };
            }
            catch (Exception ex)
            {
                return new ServiceResponse<Moto>
                {
                    Success = false,
                    Message = $"Erro ao atualizar moto: {ex.Message}"
                };
            }
        }

        public async Task<ServiceResponse<Moto>> DeleteMotoAsync(long id)
        {
            try
            {
                Moto? moto = await _motoRepository.GetByIdAsync(id);
                if (moto == null)
                {
                    return new ServiceResponse<Moto>
                    {
                        Success = false,
                        Message = "Moto não encontrada."
                    };
                }
                await _motoRepository.RemoveAsync(moto);
                await _motoRepository.SaveChangesAsync();
                return new ServiceResponse<Moto>
                {
                    Success = true,
                    Data = moto
                };
            }
            catch (Exception ex)
            {
                return new ServiceResponse<Moto>
                {
                    Success = false,
                    Message = $"Erro ao excluir moto: {ex.Message}"
                };
            }
        }

        public async Task<ServiceResponse<Moto>> CadastrarImagemReferenciaAsync(long id, string imagemReferencia)
        {
            try
            {
                Moto? moto = await _motoRepository.GetByIdAsync(id);
                if (moto == null)
                {
                    return new ServiceResponse<Moto>
                    {
                        Success = false,
                        Message = "Moto não encontrada."
                    };
                }

                moto.AtualizarImagemReferencia(imagemReferencia);

                await _motoRepository.UpdateAsync(moto);
                await _motoRepository.SaveChangesAsync();

                return new ServiceResponse<Moto>
                {
                    Success = true,
                    Data = moto
                };
            }
            catch (Exception ex)
            {
                return new ServiceResponse<Moto>
                {
                    Success = false,
                    Message = $"Erro ao cadastrar imagem de referência: {ex.Message}"
                };
            }
        }
    }
}


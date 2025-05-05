
using Microsoft.EntityFrameworkCore;
using Trackin.API.Domain.Entity;
using Trackin.API.Domain.Enums;
using Trackin.API.DTOs;
using Trackin.API.Infrastructure.Context;

namespace Trackin.API.Services
{
    public class MotoService
    {
        private readonly TrackinContext _db;
        public MotoService(TrackinContext db)
        {
            _db = db;
        }

        public async Task<ServiceResponse<MotoDTO>> CreateMotoAsync(MotoDTO motoDTO)
        {
            try
            {
                Moto moto = new(motoDTO.PatioId, motoDTO.Placa, motoDTO.Modelo, motoDTO.Ano, motoDTO.RFIDTag);

                await _db.Motos.AddAsync(moto);
                await _db.SaveChangesAsync();

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
                Moto? moto = await _db.Motos.FindAsync(id);
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

        public async Task<ServiceResponse<List<Moto>>> GetAllMotosAsync()
        {
            try
            {
                List<Moto> motos = await _db.Motos.ToListAsync();
                return new ServiceResponse<List<Moto>>
                {
                    Success = true,
                    Data = motos
                };
            }
            catch (Exception ex)
            {
                return new ServiceResponse<List<Moto>>
                {
                    Success = false,
                    Message = $"Erro ao obter motos: {ex.Message}"
                };
            }
        }

        public async Task<ServiceResponse<List<Moto>>> GetAllMotosByPatioAsync(long patioId)
        {
            try
            {
                List<Moto> motos = await _db.Motos
                    .Where(m => m.PatioId == patioId)
                    .ToListAsync();
                return new ServiceResponse<List<Moto>>
                {
                    Success = true,
                    Data = motos
                };
            }
            catch (Exception ex)
            {
                return new ServiceResponse<List<Moto>>
                {
                    Success = false,
                    Message = $"Erro ao obter motos por filial: {ex.Message}"
                };
            }
        }

        public async Task<ServiceResponse<List<Moto>>> GetAllMotosByStatusAsync(MotoStatus status)
        {
            try
            {
                List<Moto> motos = await _db.Motos
                    .Where(m => m.Status == status)
                    .ToListAsync();
                return new ServiceResponse<List<Moto>>
                {
                    Success = true,
                    Data = motos
                };
            }
            catch (Exception ex)
            {
                return new ServiceResponse<List<Moto>>
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
                Moto? moto = await _db.Motos.FindAsync(id);
                if (moto == null)
                {
                    return new ServiceResponse<Moto>
                    {
                        Success = false,
                        Message = "Moto não encontrada."
                    };
                }

                _db.Entry(moto).CurrentValues.SetValues(motoDTO);

                await _db.SaveChangesAsync();
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
                Moto? moto = await _db.Motos.FindAsync(id);
                if (moto == null)
                {
                    return new ServiceResponse<Moto>
                    {
                        Success = false,
                        Message = "Moto não encontrada."
                    };
                }
                _db.Motos.Remove(moto);
                await _db.SaveChangesAsync();
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
                Moto? moto = await _db.Motos.FindAsync(id);
                if (moto == null)
                {
                    return new ServiceResponse<Moto>
                    {
                        Success = false,
                        Message = "Moto não encontrada."
                    };
                }
                moto.AtualizarImagemReferencia(imagemReferencia);
                _db.Motos.Update(moto);
                await _db.SaveChangesAsync();
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


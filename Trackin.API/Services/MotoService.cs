
using Trackin.API.Domain.Entity;
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
                Moto moto = new(motoDTO.Placa, motoDTO.Modelo, motoDTO.Ano, motoDTO.RFIDTag);

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
    }
}

//GET / api / motos - Listar todas as motos
//GET / api / motos /{ id}
//-Obter moto específica
//GET /api/motos/filial/{filialId} -Listar motos por filial
//GET /api/motos/status/{status} -Listar motos por status
//POST /api/motos - Cadastrar nova moto
//PUT /api/motos/{id} -Atualizar moto
//DELETE /api/motos/{id} -Excluir moto(soft delete)
//POST / api / motos /{ id}/ imagem - referencia - Cadastrar imagem de referência para reconhecimento

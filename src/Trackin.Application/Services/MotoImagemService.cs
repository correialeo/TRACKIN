using Trackin.Application.Common;
using Trackin.Application.Interfaces;
using Trackin.Domain.Entity;
using Trackin.Domain.Interfaces;
using System;
using System.Threading.Tasks;

namespace Trackin.Application.Services
{
    public class MotoImagemService : IMotoImagemService
    {
        private readonly IMotoRepository _motoRepository;

        public MotoImagemService(IMotoRepository motoRepository)
        {
            _motoRepository = motoRepository;
        }

        private async Task<Moto?> ObterMoto(long id) => await _motoRepository.GetByIdAsync(id);

        private ServiceResponse<T> Sucesso<T>(T data, string message = "") => new() { Success = true, Data = data, Message = message };
        private ServiceResponse<T> Erro<T>(string message) => new() { Success = false, Message = message };

        public async Task<ServiceResponse<Moto>> CadastrarImagemReferenciaAsync(long motoId, string imagemReferencia)
        {
            try
            {
                var moto = await ObterMoto(motoId);
                if (moto == null)
                    return Erro<Moto>("Moto não encontrada");

                moto.AtualizarImagemReferencia(imagemReferencia);

                await _motoRepository.UpdateAsync(moto);
                await _motoRepository.SaveChangesAsync();

                return Sucesso(moto);
            }
            catch (Exception ex)
            {
                return Erro<Moto>($"Erro ao cadastrar imagem de referência: {ex.Message}");
            }
        }
    }
}
using Trackin.Application.Common;
using Trackin.Application.Interfaces;
using Trackin.Domain.Entity;
using Trackin.Domain.Interfaces;
using Trackin.Domain.ValueObjects;

namespace Trackin.Application.Services
{
    public class PatioValidacaoService : IPatioValidacaoService
    {
        private readonly IPatioRepository _patioRepository;
        private const string PatioNaoExiste = "Não existe pátio cadastrado com id informado.";

        public PatioValidacaoService(IPatioRepository patioRepository)
        {
            _patioRepository = patioRepository;
        }

        private async Task<Patio?> ObterPatio(long id) => await _patioRepository.GetByIdAsync(id);
        private ServiceResponse<T> Sucesso<T>(T data, string message = "") => new() { Success = true, Data = data, Message = message };
        private ServiceResponse<T> Erro<T>(string message) => new() { Success = false, Message = message };

        public async Task<ServiceResponse<bool>> PodeRemoverPatioAsync(long patioId)
        {
            try
            {
                var patio = await ObterPatio(patioId);
                if (patio == null) return Erro<bool>(PatioNaoExiste);

                bool pode = !(patio.Cameras.Any() || patio.Zonas.Any() || patio.SensoresRFID.Any());
                if (!pode) return Erro<bool>("Não é possível remover um pátio que possui câmeras, zonas ou sensores associados.");

                return Sucesso(true);
            }
            catch (Exception ex)
            {
                return Erro<bool>($"Erro ao verificar remoção do pátio: {ex.Message}");
            }
        }

        public async Task<ServiceResponse<bool>> ValidarCoordenadaAsync(long patioId, double x, double y)
        {
            try
            {
                var patio = await ObterPatio(patioId);
                if (patio == null) return Erro<bool>(PatioNaoExiste);

                Coordenada coordenada = new Coordenada(x, y);
                bool valida = patio.CoordenadaEstaValida(coordenada);
                return Sucesso(valida);
            }
            catch (Exception ex)
            {
                return Erro<bool>($"Erro ao validar coordenada: {ex.Message}");
            }
        }
    }
}

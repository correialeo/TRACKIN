using Trackin.Application.Common;
using Trackin.Application.Interfaces;
using Trackin.Domain.Entity;
using Trackin.Domain.Interfaces;

namespace Trackin.Application.Services
{
    public class PatioMetricasService : IPatioMetricasService
    {
        private readonly IPatioRepository _patioRepository;
        private const string PatioNaoExiste = "Não existe pátio cadastrado com id informado.";

        public PatioMetricasService(IPatioRepository patioRepository)
        {
            _patioRepository = patioRepository;
        }

        private async Task<Patio> ObterPatio(long id) => await _patioRepository.GetByIdAsync(id);
        private ServiceResponse<T> Sucesso<T>(T data, string message = "") => new() { Success = true, Data = data, Message = message };
        private ServiceResponse<T> Erro<T>(string message) => new() { Success = false, Message = message };

        public async Task<ServiceResponse<double>> GetTaxaOcupacaoAsync(long patioId)
        {
            try
            {
                var patio = await ObterPatio(patioId);
                if (patio == null) return Erro<double>(PatioNaoExiste);

                double taxa = patio.CalcularTaxaOcupacao();
                return Sucesso(taxa);
            }
            catch (Exception ex)
            {
                return Erro<double>($"Erro ao calcular taxa de ocupação: {ex.Message}");
            }
        }
    }
}
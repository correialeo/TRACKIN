// IPatioMetricasService.cs
using Trackin.Application.Common;

namespace Trackin.Application.Interfaces
{
    public interface IPatioMetricasService
    {
        Task<ServiceResponse<double>> GetTaxaOcupacaoAsync(long patioId);
    }
}

using Trackin.Application.Common;
using Trackin.Domain.ValueObjects;

namespace Trackin.Application.Interfaces
{
    public interface IPatioValidacaoService
    {
        Task<ServiceResponse<bool>> PodeRemoverPatioAsync(long patioId);
        Task<ServiceResponse<bool>> ValidarCoordenadaAsync(long patioId, double x, double y);
    }
}
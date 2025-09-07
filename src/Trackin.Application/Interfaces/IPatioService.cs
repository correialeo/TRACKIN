using Trackin.Application.Common;
using Trackin.Application.DTOs;
using Trackin.Domain.Entity;

namespace Trackin.Application.Interfaces
{
    public interface IPatioService
    {
        Task<ServiceResponsePaginado<Patio>> GetAllPatiosPaginatedAsync(
            int pageNumber,
            int pageSize,
            string? ordering = null,
            bool descendingOrder = false);
        Task<ServiceResponse<IEnumerable<Patio>>> GetAllPatiosAsync();
        Task<ServiceResponse<Patio>> GetPatioByIdAsync(long id);
        Task<ServiceResponse<Patio>> CreatePatioAsync(CriarPatioDto dto);
        Task<ServiceResponse<Patio>> DeletePatioAsync(long id);

    }
}

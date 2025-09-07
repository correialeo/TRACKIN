using Trackin.Application.Common;
using Trackin.Application.DTOs;
using Trackin.Domain.Entity;

namespace Trackin.Application.Interfaces
{
    public interface IZonaPatioService
    {
        Task<ServiceResponsePaginado<ZonaPatio>> GetAllZonasPatiosPaginatedAsync(
            int pageNumber,
            int pageSize,
            string? ordering = null,
            bool descendingOrder = false);
        Task<ServiceResponse<IEnumerable<ZonaPatio>>> GetAllZonasPatiosAsync();
        Task<ServiceResponse<ZonaPatio>> GetZonaPatioByIdAsync(long id);
        Task<ServiceResponse<ZonaPatio>> CreateZonaPatioAsync(CriarZonaPatioDTO dto);
        Task<ServiceResponse<ZonaPatio>> UpdateZonaPatioAsync(long id, CriarZonaPatioDTO zonaPatioDto);
        Task<ServiceResponse<ZonaPatio>> DeleteZonaPatioAsync(long id);
    }
}

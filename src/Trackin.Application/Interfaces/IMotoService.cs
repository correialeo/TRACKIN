using Trackin.Application.Common;
using Trackin.Application.DTOs;
using Trackin.Domain.Entity;
using Trackin.Domain.Enums;

namespace Trackin.Application.Interfaces
{
    public interface IMotoService
    {
        Task<ServiceResponse<MotoDTO>> CreateMotoAsync(MotoDTO motoDTO);
        Task<ServiceResponse<Moto>> GetMotoByIdAsync(long id);
        Task<ServiceResponse<IEnumerable<Moto>>> GetAllMotosAsync();
        Task<ServiceResponsePaginado<Moto>> GetAllMotosPaginatedAsync(
            int pageNumber,
            int pageSize,
            string? ordering = null,
            bool descendingOrder = false);
        Task<ServiceResponsePaginado<Moto>> GetMotosByPatioPaginatedAsync(
            long patioId,
            int pageNumber,
            int pageSize,
            string? ordering = null,
            bool descendingOrder = false);
        Task<ServiceResponsePaginado<Moto>> GetMotosByStatusPaginatedAsync(
            MotoStatus status,
            int pageNumber,
            int pageSize,
            string? ordering = null,
            bool descendingOrder = false);
        Task<ServiceResponse<Moto>> UpdateMotoAsync(long id, EditarMotoDTO motoDto);
        Task<ServiceResponse<Moto>> DeleteMotoAsync(long id);
    }
}

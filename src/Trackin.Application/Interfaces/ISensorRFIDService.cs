using Trackin.Application.Common;
using Trackin.Application.DTOs;
using Trackin.Domain.Entity;

namespace Trackin.Application.Interfaces
{
    public interface ISensorRFIDService
    {
        Task<ServiceResponsePaginado<SensorRFID>> GetAllSensoresRFIDPaginatedAsync(
            int pageNumber,
            int pageSize,
            string? ordering = null,
            bool descendingOrder = false);
        Task<ServiceResponse<IEnumerable<SensorRFID>>> GetAllSensoresRFIDAsync();
        Task<ServiceResponse<SensorRFID>> GetSensorRFIDByIdAsync(long id);
        Task<ServiceResponse<SensorRFID>> CreateSensorRFIDAsync(CriarSensorRFIdDTO sensorRFIDDTO);
        Task<ServiceResponse<SensorRFID>> UpdateSensorRFIDAsync(long id, CriarSensorRFIdDTO sensorRFIDDTO);
        Task<ServiceResponse<SensorRFID>> DeleteSensorRFIDAsync(long id);
    }
}

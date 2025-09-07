using Trackin.Domain.Entity;

namespace Trackin.Domain.Interfaces
{
    public interface ISensorRFIDRepository : IRepository<SensorRFID>
    {
        Task<SensorRFID> GetSensorWithZonaAndPatioAsync(long sensorId);
    }
}

using Trackin.API.Domain.Entity;

namespace Trackin.API.Infrastructure.Persistence.Repositories
{
    public interface ISensorRFIDRepository : IRepository<SensorRFID>
    {
        Task<SensorRFID> GetSensorWithZonaAndPatioAsync(long sensorId);
    }
}

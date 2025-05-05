using Microsoft.EntityFrameworkCore;
using Trackin.API.Domain.Entity;
using Trackin.API.Infrastructure.Context;

namespace Trackin.API.Infrastructure.Persistence.Repositories
{
    public class SensorRFIDRepository : Repository<SensorRFID>, ISensorRFIDRepository
    {
        public SensorRFIDRepository(TrackinContext context) : base(context)
        {
        }

        public async Task<SensorRFID> GetSensorWithZonaAndPatioAsync(long sensorId)
        {
            return await _dbSet
                .Include(s => s.ZonaPatio)
                .ThenInclude(z => z.Patio)
                .FirstOrDefaultAsync(s => s.Id == sensorId);
        }
    }
}

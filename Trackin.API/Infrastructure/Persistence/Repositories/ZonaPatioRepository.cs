using Trackin.API.Domain.Entity;
using Trackin.API.Infrastructure.Context;

namespace Trackin.API.Infrastructure.Persistence.Repositories
{
    public class ZonaPatioRepository : Repository<ZonaPatio>, IZonaPatioRepository
    {
        public ZonaPatioRepository(TrackinContext context) : base(context)
        {
        }
    }
}

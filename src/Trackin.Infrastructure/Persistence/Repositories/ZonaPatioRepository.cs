using Trackin.Domain.Entity;
using Trackin.Domain.Interfaces;
using Trackin.Infrastructure.Context;

namespace Trackin.Infrastructure.Persistence.Repositories
{
    public class ZonaPatioRepository : Repository<ZonaPatio>, IZonaPatioRepository
    {
        public ZonaPatioRepository(TrackinContext context) : base(context)
        {
        }
    }
}

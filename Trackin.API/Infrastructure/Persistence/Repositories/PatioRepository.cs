using Trackin.API.Domain.Entity;
using Trackin.API.Infrastructure.Context;

namespace Trackin.API.Infrastructure.Persistence.Repositories
{
    public class PatioRepository : Repository<Patio>, IPatioRepository
    {
        public PatioRepository(TrackinContext context) : base(context)
        {
        }
    }
}

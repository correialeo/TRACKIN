using Trackin.Domain.Entity;
using Trackin.Domain.Interfaces;
using Trackin.Infrastructure.Context;

namespace Trackin.Infrastructure.Persistence.Repositories
{
    public class PatioRepository : Repository<Patio>, IPatioRepository
    {
        public PatioRepository(TrackinContext context) : base(context)
        {
        }
    }
}

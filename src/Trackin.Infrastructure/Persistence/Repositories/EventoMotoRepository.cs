using Trackin.Domain.Entity;
using Trackin.Domain.Interfaces;
using Trackin.Infrastructure.Context;

namespace Trackin.Infrastructure.Persistence.Repositories
{
    public class EventoMotoRepository : Repository<EventoMoto>, IEventoMotoRepository
    {
        public EventoMotoRepository(TrackinContext context) : base(context)
        {
        }

    }
}

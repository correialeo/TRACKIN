using Trackin.API.Domain.Entity;
using Trackin.API.Infrastructure.Context;

namespace Trackin.API.Infrastructure.Persistence.Repositories
{
    public class EventoMotoRepository : Repository<EventoMoto>, IEventoMotoRepository
    {
        public EventoMotoRepository(TrackinContext context) : base(context)
        {
        }

    }
}

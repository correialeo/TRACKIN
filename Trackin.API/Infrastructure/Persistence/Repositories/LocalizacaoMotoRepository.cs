using Trackin.API.Domain.Entity;
using Trackin.API.Infrastructure.Context;

namespace Trackin.API.Infrastructure.Persistence.Repositories
{
    public class LocalizacaoMotoRepository : Repository<LocalizacaoMoto>, ILocalizacaoMotoRepository
    {
        public LocalizacaoMotoRepository(TrackinContext context) : base(context)
        {
        }

    }
}

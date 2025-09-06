
using Trackin.Domain.Entity;
using Trackin.Domain.Interfaces;
using Trackin.Infrastructure.Context;

namespace Trackin.Infrastructure.Persistence.Repositories
{
    public class LocalizacaoMotoRepository : Repository<LocalizacaoMoto>, ILocalizacaoMotoRepository
    {
        public LocalizacaoMotoRepository(TrackinContext context) : base(context)
        {
        }

    }
}

using Microsoft.EntityFrameworkCore;
using Trackin.Domain.Entity;
using Trackin.Domain.Enums;
using Trackin.Domain.Interfaces;
using Trackin.Infrastructure.Context;

namespace Trackin.Infrastructure.Persistence.Repositories
{
    public class MotoRepository : Repository<Moto>, IMotoRepository
    {
        public MotoRepository(TrackinContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Moto>> GetAllByPatioAsync(long patioId)
        {
            return await _dbSet.Where(m => m.PatioId == patioId).ToListAsync();
        }

        public async Task<IEnumerable<Moto>> GetAllByStatusAsync(MotoStatus status)
        {
            return await _dbSet.Where(m => m.Status == status).ToListAsync();
        }

        public async Task<Moto> GetByRFIDTagAsync(string rfidTag)
        {
            return await _dbSet.FirstOrDefaultAsync(m => m.RFIDTag == rfidTag);
        }

        public async Task<Moto> UpdateMotoAsync(Moto moto)
        {
            _dbSet.Update(moto);
            await _context.SaveChangesAsync();
            return moto;
        }
    }
}

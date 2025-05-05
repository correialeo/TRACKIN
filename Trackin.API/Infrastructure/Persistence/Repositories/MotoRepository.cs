using Microsoft.EntityFrameworkCore;
using Trackin.API.Domain.Entity;
using Trackin.API.Domain.Enums;
using Trackin.API.DTOs;
using Trackin.API.Infrastructure.Context;

namespace Trackin.API.Infrastructure.Persistence.Repositories
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

        public async Task<Moto> UpdateMotoAsync(Moto moto, EditarMotoDTO motoDTO)
        {
            _dbSet.Entry(moto).CurrentValues.SetValues(motoDTO);
            await _context.SaveChangesAsync();
            return moto;
        }
    }
}

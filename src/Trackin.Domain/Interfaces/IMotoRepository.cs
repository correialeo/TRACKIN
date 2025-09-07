using Trackin.Domain.Entity;
using Trackin.Domain.Enums;

namespace Trackin.Domain.Interfaces
{
    public interface IMotoRepository : IRepository<Moto>
    {
        Task<IEnumerable<Moto>> GetAllByPatioAsync(long patioId);
        Task<IEnumerable<Moto>> GetAllByStatusAsync(MotoStatus status);
        Task<Moto> GetByRFIDTagAsync(string rfidTag);
        //update async recebendo moto e motodto
        Task<Moto> UpdateMotoAsync(Moto moto);
    }
}
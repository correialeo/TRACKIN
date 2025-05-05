
using Trackin.API.Domain.Entity;
using Trackin.API.Domain.Enums;
using Trackin.API.DTOs;

namespace Trackin.API.Infrastructure.Persistence.Repositories
{
    public interface IMotoRepository : IRepository<Moto>
    {
        Task<IEnumerable<Moto>> GetAllByPatioAsync(long patioId);
        Task<IEnumerable<Moto>> GetAllByStatusAsync(MotoStatus status);
        Task<Moto> GetByRFIDTagAsync(string rfidTag);
        //update async recebendo moto e motodto
        Task<Moto> UpdateMotoAsync(Moto moto, EditarMotoDTO motoDto);
    }
}
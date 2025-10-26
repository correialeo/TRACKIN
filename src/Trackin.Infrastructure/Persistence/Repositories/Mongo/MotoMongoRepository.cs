using MongoDB.Driver;
using Trackin.Domain.Entity;
using Trackin.Domain.Enums;
using Trackin.Domain.Interfaces;
using Trackin.Infrastructure.Context;

namespace Trackin.Infrastructure.Persistence.Repositories.Mongo
{
    /// <summary>
    /// Repositório MongoDB para Moto
    /// </summary>
    public class MotoMongoRepository : IMotoRepository
    {
        private readonly TrackinMongoContext _context;
        private readonly IMongoCollection<Moto> _collection;

        public MotoMongoRepository(TrackinMongoContext context)
        {
            _context = context;
            _collection = context.Motos;
        }

        public async Task<Moto?> GetByIdAsync(long id)
        {
            return await _collection.Find(m => m.Id == id).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Moto>> GetAllAsync()
        {
            return await _collection.Find(_ => true).ToListAsync();
        }

        public async Task<IEnumerable<Moto>> FindAsync(System.Linq.Expressions.Expression<Func<Moto, bool>> predicate)
        {
            return await _collection.Find(predicate).ToListAsync();
        }

        public async Task<(IEnumerable<Moto> Items, int TotalCount)> GetAllPaginatedAsync(
            int pageNumber, 
            int pageSize, 
            string? ordering = null, 
            bool descendingOrder = false)
        {
            var filter = Builders<Moto>.Filter.Empty;
            var query = _collection.Find(filter);

            if (!string.IsNullOrEmpty(ordering))
            {
                var sortDefinition = ordering.ToLower() switch
                {
                    "placa" => descendingOrder 
                        ? Builders<Moto>.Sort.Descending(m => m.Placa) 
                        : Builders<Moto>.Sort.Ascending(m => m.Placa),
                    "modelo" => descendingOrder 
                        ? Builders<Moto>.Sort.Descending(m => m.Modelo) 
                        : Builders<Moto>.Sort.Ascending(m => m.Modelo),
                    "ano" => descendingOrder 
                        ? Builders<Moto>.Sort.Descending(m => m.Ano) 
                        : Builders<Moto>.Sort.Ascending(m => m.Ano),
                    _ => Builders<Moto>.Sort.Ascending(m => m.Id)
                };
                query = query.Sort(sortDefinition);
            }

            long totalCount = await _collection.CountDocumentsAsync(filter);
            var items = await query
                .Skip((pageNumber - 1) * pageSize)
                .Limit(pageSize)
                .ToListAsync();

            return (Items: items, TotalCount: (int)totalCount);
        }

        public async Task AddAsync(Moto entity)
        {
            await _collection.InsertOneAsync(entity);
        }

        public async Task UpdateMotoAsync(Moto entity)
        {
            await _collection.ReplaceOneAsync(m => m.Id == entity.Id, entity);
        }

        public async Task RemoveAsync(Moto entity)
        {
            await _collection.DeleteOneAsync(m => m.Id == entity.Id);
        }

        public async Task SaveChangesAsync()
        {
            await Task.CompletedTask;
        }

        public async Task<Moto?> GetByPlacaAsync(string placa)
        {
            return await _collection.Find(m => m.Placa == placa).FirstOrDefaultAsync();
        }

        public async Task<Moto?> GetByRFIDTagAsync(string rfidTag)
        {
            return await _collection.Find(m => m.RFIDTag == rfidTag).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Moto>> GetByPatioIdAsync(long patioId)
        {
            return await _collection.Find(m => m.PatioId == patioId).ToListAsync();
        }

        public async Task<IEnumerable<Moto>> GetByStatusAsync(Domain.Enums.MotoStatus status)
        {
            return await _collection.Find(m => m.Status == status).ToListAsync();
        }

        public Task<IEnumerable<Moto>> GetAllByPatioAsync(long patioId)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Moto>> GetAllByStatusAsync(MotoStatus status)
        {
            throw new NotImplementedException();
        }

        Task<Moto> IMotoRepository.UpdateMotoAsync(Moto moto)
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(Moto entity)
        {
            throw new NotImplementedException();
        }
    }
}

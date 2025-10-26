using MongoDB.Driver;
using Trackin.Domain.Entity;
using Trackin.Domain.Interfaces;
using Trackin.Infrastructure.Context;

namespace Trackin.Infrastructure.Persistence.Repositories.Mongo
{
    /// <summary>
    /// Repositório MongoDB para Patio
    /// </summary>
    public class PatioMongoRepository : IPatioRepository
    {
        private readonly TrackinMongoContext _context;
        private readonly IMongoCollection<Patio> _collection;

        public PatioMongoRepository(TrackinMongoContext context)
        {
            _context = context;
            _collection = context.Patios;
        }

        public async Task<Patio?> GetByIdAsync(long id)
        {
            return await _collection.Find(p => p.Id == id).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Patio>> GetAllAsync()
        {
            try

            {
                return await _collection.Find(_ => true).ToListAsync();

            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao obter todos os pátios: " + ex.Message, ex);
            }
        }

        public async Task<IEnumerable<Patio>> FindAsync(System.Linq.Expressions.Expression<Func<Patio, bool>> predicate)
        {
            return await _collection.Find(predicate).ToListAsync();
        }

        public async Task<(IEnumerable<Patio> Items, int TotalCount)> GetAllPaginatedAsync(
            int pageNumber, 
            int pageSize, 
            string? ordering = null, 
            bool descendingOrder = false)
        {
            FilterDefinition<Patio> filter = Builders<Patio>.Filter.Empty;
            IFindFluent<Patio, Patio> query = _collection.Find(filter);

            if (!string.IsNullOrEmpty(ordering))
            {
                SortDefinition<Patio> sortDefinition = ordering.ToLower() switch
                {
                    "nome" => descendingOrder 
                        ? Builders<Patio>.Sort.Descending(p => p.Nome) 
                        : Builders<Patio>.Sort.Ascending(p => p.Nome),
                    "cidade" => descendingOrder 
                        ? Builders<Patio>.Sort.Descending(p => p.Cidade) 
                        : Builders<Patio>.Sort.Ascending(p => p.Cidade),
                    "estado" => descendingOrder 
                        ? Builders<Patio>.Sort.Descending(p => p.Estado) 
                        : Builders<Patio>.Sort.Ascending(p => p.Estado),
                    _ => Builders<Patio>.Sort.Ascending(p => p.Id)
                };
                query = query.Sort(sortDefinition);
            }

            long totalCount = await _collection.CountDocumentsAsync(filter);
            List<Patio> items = await query
                .Skip((pageNumber - 1) * pageSize)
                .Limit(pageSize)
                .ToListAsync();

            return (items, (int)totalCount);
        }

        public async Task AddAsync(Patio entity)
        {
            await _collection.InsertOneAsync(entity);
        }

        public async Task UpdateAsync(Patio entity)
        {
            await _collection.ReplaceOneAsync(p => p.Id == entity.Id, entity);
        }

        public async Task RemoveAsync(Patio entity)
        {
            await _collection.DeleteOneAsync(p => p.Id == entity.Id);
        }

        public async Task SaveChangesAsync()
        {
            await Task.CompletedTask;
        }

        public async Task<Patio?> GetByNomeAsync(string nome)
        {
            return await _collection.Find(p => p.Nome == nome).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Patio>> GetByCidadeAsync(string cidade)
        {
            return await _collection.Find(p => p.Cidade == cidade).ToListAsync();
        }

        public async Task<IEnumerable<Patio>> GetByEstadoAsync(string estado)
        {
            return await _collection.Find(p => p.Estado == estado).ToListAsync();
        }
    }
}

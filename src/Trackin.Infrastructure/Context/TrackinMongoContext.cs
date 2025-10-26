using MongoDB.Driver;
using Trackin.Domain.Entity;

namespace Trackin.Infrastructure.Context
{
    public class TrackinMongoContext
    {
        private readonly IMongoDatabase _database;

        public TrackinMongoContext(IMongoDatabase database)
        {
            _database = database;
        }

        public IMongoCollection<Moto> Motos => _database.GetCollection<Moto>("motos");
        public IMongoCollection<Patio> Patios => _database.GetCollection<Patio>("patios");
        public IMongoCollection<LocalizacaoMoto> Localizacoes => _database.GetCollection<LocalizacaoMoto>("localizacoes");
        public IMongoCollection<EventoMoto> Eventos => _database.GetCollection<EventoMoto>("eventos");
        public IMongoCollection<SensorRFID> SensoresRFID => _database.GetCollection<SensorRFID>("sensores_rfid");
        public IMongoCollection<ZonaPatio> ZonasPatio => _database.GetCollection<ZonaPatio>("zonas_patio");
        public IMongoCollection<Camera> Cameras => _database.GetCollection<Camera>("cameras");
        public IMongoCollection<Usuario> Usuarios => _database.GetCollection<Usuario>("usuarios");
        public IMongoCollection<DeteccaoVisual> DeteccoesVisuais => _database.GetCollection<DeteccaoVisual>("deteccoes_visuais");
    }
}

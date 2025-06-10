using InventoryManagementSystem.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace InventoryManagementSystem.Services
{
    public class ActivityLogService
    {
        private readonly IMongoCollection<ActivityLog> _logCollection;

        public ActivityLogService(IOptions<MongoDbSettings> settings)
        {
            var client = new MongoClient(settings.Value.ConnectionString);
            var database = client.GetDatabase(settings.Value.DatabaseName);
            _logCollection = database.GetCollection<ActivityLog>("ActivityLogs");
        }

        public async Task LogAsync(string action, string performedBy, string entity, string entityId)
        {
            var log = new ActivityLog
            {
                Action = action,
                PerformedBy = performedBy,
                Entity = entity,
                EntityId = entityId
            };

            await _logCollection.InsertOneAsync(log);
        }

        public async Task<List<ActivityLog>> GetLogsAsync() =>
            await _logCollection.Find(_ => true).SortByDescending(l => l.Timestamp).ToListAsync();
    }

}

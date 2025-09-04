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
                EntityId = entityId,
                Timestamp = DateTime.UtcNow
            };

            await _logCollection.InsertOneAsync(log);
        }

        public async Task<List<ActivityLog>> GetAllLogsAsync() =>
            await _logCollection.Find(_ => true)
                                .SortByDescending(l => l.Timestamp)
                                .ToListAsync();

        public async Task<List<ActivityLog>> GetLogsByUserAsync(string username) =>
        await _logCollection.Find(l => l.PerformedBy == username)
                            .SortByDescending(l => l.Timestamp)
                            .ToListAsync();

        public async Task<List<ActivityLog>> GetLogsByEntityAsync(string entity, string entityId) =>
        await _logCollection.Find(l => l.Entity == entity && l.EntityId == entityId)
                                .SortByDescending(l => l.Timestamp)
                                .ToListAsync();

    }

}

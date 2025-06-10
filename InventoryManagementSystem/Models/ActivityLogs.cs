using MongoDB.Bson;

namespace InventoryManagementSystem.Models
{
    public class ActivityLog
    {
        public ObjectId Id { get; set; }
        public string Action { get; set; } = null!;
        public string PerformedBy { get; set; } = null!;
        public string Entity { get; set; } = null!;
        public string EntityId { get; set; } = null!;
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;
    }

}

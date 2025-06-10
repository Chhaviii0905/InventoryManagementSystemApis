namespace InventoryManagementSystem.DTOs
{
    public class AuditLogDto
    {
        public string Action { get; set; } = string.Empty;  // e.g., "Product Created"
        public string Username { get; set; } = string.Empty;
        public DateTime Timestamp { get; set; }
        public string Details { get; set; } = string.Empty;
    }
}

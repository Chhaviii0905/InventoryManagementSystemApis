namespace InventoryManagementSystem.DTOs
{
    public class RecentOrderDto
    {
        public int OrderId { get; set; }
        public string Status { get; set; } = "Pending";
        public int ItemsCount { get; set; }
        public DateTime? OrderDate { get; set; }
        public string? CustomerName { get; set; }
        public List<OrderItemDto> Items { get; set; } = new();
    }
}

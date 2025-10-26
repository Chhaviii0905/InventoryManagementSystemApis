namespace InventoryManagementSystem.DTOs
{
    public class OrderDto
    {
        public int OrderId { get; set; }
        public DateTime? OrderDate { get; set; }
        public int? UserId { get; set; }
        public string? UserName { get; set; }
        public string Status { get; set; } = "Pending";

        public int? CustomerId { get; set; }
        public string? CustomerName { get; set; }
        public List<OrderItemDto> Items { get; set; } = new();
    }

    public class CreateOrderDto
    {
        public int UserId { get; set; }
        public int CustomerId { get; set; }
        public List<CreateOrderItemDto> Items { get; set; } = new();
    }
}

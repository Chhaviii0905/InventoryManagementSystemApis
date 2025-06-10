namespace InventoryManagementSystem.DTOs
{
    public class ProductDto
    {
        public int ProductId { get; set; }              // Exposed in GET
        public string Name { get; set; } = string.Empty;
        public int Quantity { get; set; }               // Current stock
        public decimal Price { get; set; }              // Unit price
    }

    public class CreateProductDto
    {
        public string Name { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public decimal Price { get; set; }
    }

    public class UpdateProductDto
    {
        public int ProductId { get; set; }
        public string Name { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public decimal Price { get; set; }
    }
}

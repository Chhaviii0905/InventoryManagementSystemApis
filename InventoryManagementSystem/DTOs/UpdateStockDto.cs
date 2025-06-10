using System.ComponentModel.DataAnnotations;

namespace InventoryManagementSystem.DTOs
{
    public class UpdateStockDto
    {
        [Required]
        public int ProductId { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "StockToAdd must be at least 1.")]
        public int StockToAdd { get; set; }  // Only positive values allowed
    }

}

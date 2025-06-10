using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InventoryManagementSystem.Models
{
    public class Customer
    {
        [Key]
        public int CustomerId { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;

        [StringLength(150)]
        public string? Email { get; set; }

        [StringLength(15)]
        public string? Phone { get; set; }

        [InverseProperty("Customer")]
        public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
    }

}

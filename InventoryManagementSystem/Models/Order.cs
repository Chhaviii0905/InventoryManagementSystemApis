using InventoryManagementSystem.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

public partial class Order
{
    [Key]
    public int OrderId { get; set; }

    public int? UserId { get; set; }

    public int? CustomerId { get; set; }

    [Column(TypeName = "timestamp")]
    public DateTime? OrderDate { get; set; }

    [ForeignKey("UserId")]
    [InverseProperty("Orders")]
    public virtual User? User { get; set; }

    [ForeignKey("CustomerId")] 
    [InverseProperty("Orders")]
    public virtual Customer? Customer { get; set; }

    [InverseProperty("Order")]
    public virtual ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
}

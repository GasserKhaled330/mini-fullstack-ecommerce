namespace Ecommerce.Core.Entities;

public class Order : BaseEntity
{
	public string CustomerName { get; set; } = string.Empty;
	public decimal TotalAmount { get; set; }
	public decimal DiscountAmount { get; set; }
	public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
	public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
}

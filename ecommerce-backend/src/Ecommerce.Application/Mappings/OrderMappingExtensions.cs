namespace Ecommerce.Application.Mappings;

public static class OrderMappingExtensions
{
	public static OrderDto ToDto(this Order order)
	{
		var subtotal = order.OrderItems.Sum(i => i.UnitPrice * i.Quantity);

		double discountPct = subtotal > 0
				? (double)(order.DiscountAmount / subtotal) * 100
				: 0;

		return new OrderDto(
				order.Id,
				order.CustomerName,
				subtotal,
				order.DiscountAmount,
				order.TotalAmount,
				Math.Round(discountPct, 1),
				order.CreatedAt,
				order.OrderItems.Select(i => new OrderItemDto(
						i.ProductId,
						i.Product?.Name ?? "Unknown Product",
						i.Quantity,
						i.UnitPrice,
						i.UnitPrice * i.Quantity
				)).ToList()
		);
	}
}

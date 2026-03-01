namespace Ecommerce.Application.Services;

public class OrderService(IApplicationDbContext context) : IOrderService
{
	public async Task<Result<OrderDto>> CreateAsync(CreateOrderRequest request)
	{
		var order = new Order { CustomerName = request.CustomerName };
		decimal subtotal = 0;
		int totalItemsCount = request.Items.Sum(i => i.Quantity);

		foreach (var itemRequest in request.Items)
		{
			var product = await context.Products.FindAsync(itemRequest.ProductId);

			if (product == null)
			{
				return Result<OrderDto>.Failure(
						$"Product {itemRequest.ProductId} not found",
						ErrorType.NotFound
				);
			}

			if (product.Quantity < itemRequest.Quantity)
			{
				return Result<OrderDto>.Failure(
						$"Insufficient stock for {product.Name}",
						ErrorType.Conflict
				);
			}

			product.Quantity -= itemRequest.Quantity;

			var orderItem = new OrderItem
			{
				ProductId = product.Id,
				Quantity = itemRequest.Quantity,
				UnitPrice = product.Price,
			};

			order.OrderItems.Add(orderItem);
			subtotal += (orderItem.UnitPrice * orderItem.Quantity);
		}

		order.DiscountAmount = CalculateDiscount(subtotal, totalItemsCount);
		order.TotalAmount = subtotal - order.DiscountAmount;

		context.Orders.Add(order);
		await context.SaveChangesAsync();

		return Result<OrderDto>.Success(order.ToDto());
	}

	public async Task<Result<OrderDto>> GetByIdAsync(int id)
	{
		var order = await context
				.Orders.Include(o => o.OrderItems)
						.ThenInclude(oi => oi.Product)
				.FirstOrDefaultAsync(o => o.Id == id);
		return order == null
				? Result<OrderDto>.Failure($"Order {id} not found", ErrorType.NotFound)
				: Result<OrderDto>.Success(order.ToDto());
	}

	private decimal CalculateDiscount(decimal subtotal, int totalQuantity)
	{
		return totalQuantity >= 5 ? subtotal * 0.10m : totalQuantity >= 2 ? subtotal * 0.05m : 0;
	}
}

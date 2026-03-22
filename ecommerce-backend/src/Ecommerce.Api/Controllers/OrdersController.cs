namespace Ecommerce.Api.Controllers;

[ApiVersion("1.0")]
public class OrdersController(IOrderService orderService) : BaseApiController
{
	/// <summary>
	/// Creates a new order.
	/// </summary>
	[HttpPost]
	[ProducesResponseType(typeof(OrderDto), StatusCodes.Status201Created)]
	[ProducesResponseType(StatusCodes.Status409Conflict)]
	public async Task<IActionResult> Create([FromBody] CreateOrderRequest request)
	{
		var result = await orderService.CreateAsync(request);

		return HandleCreated(
				result,
				nameof(GetById),
				new { id = result.Value?.Id }
		);
	}

	/// <summary>
	/// Retrieves a specific order by its ID.
	/// </summary>
	[HttpGet("{id:int}", Name = nameof(GetById))]
	[ProducesResponseType(typeof(OrderDto), StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	public async Task<IActionResult> GetById(int id)
	{
		var result = await orderService.GetByIdAsync(id);
		return HandleResult(result);
	}
}

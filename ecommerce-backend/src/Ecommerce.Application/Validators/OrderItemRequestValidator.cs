namespace Ecommerce.Application.Validators;

public class OrderItemRequestValidator : AbstractValidator<OrderItemRequest>
{
	public OrderItemRequestValidator()
	{
		RuleFor(x => x.ProductId)
				.GreaterThan(0).WithMessage("A valid Product ID is required.");

		RuleFor(x => x.Quantity)
				.GreaterThan(0).WithMessage("Quantity must be at least 1.");
	}
}

namespace Ecommerce.Application.Validators
{
	public class CreateOrderRequestValidator : AbstractValidator<CreateOrderRequest>
	{
		public CreateOrderRequestValidator()
		{
			// 1. Customer Name Rules
			RuleFor(x => x.CustomerName)
					.NotEmpty()
					.WithMessage("Customer name is required.")
					.MaximumLength(100)
					.WithMessage("Customer name cannot exceed 100 characters.");

			// 2. Items List Rules
			RuleFor(x => x.Items)
					.NotEmpty()
					.WithMessage("An order must contain at least one item.")
					.Must(items => items != null && items.Count > 0)
					.WithMessage("The order items list cannot be empty.")
					.Must(items => items.Select(i => i.ProductId).Distinct().Count() == items.Count)
					.WithMessage("Duplicate products found in order. Please consolidate quantities.");

			// 3. Individual Item Rules (Nested Validation)
			RuleForEach(x => x.Items).SetValidator(new OrderItemRequestValidator());
		}
	}
}

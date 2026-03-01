namespace Ecommerce.Application.Interfaces;

public interface IApplicationDbContext
{
	DbSet<Product> Products { get; }
	DbSet<Order> Orders { get; }
	DbSet<OrderItem> OrderItems { get; }
	Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}

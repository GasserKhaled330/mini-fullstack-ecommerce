namespace Ecommerce.Application.Services;

public class ProductService(IApplicationDbContext context) : IProductService
{
	public async Task<Result<ProductDto>> GetByIdAsync(int id)
	{
		var product = await context.Products.FindAsync(id);

		return product is null
				? Result<ProductDto>.Failure("Product not found", ErrorType.NotFound)
				: Result<ProductDto>.Success(product.ToDto());
	}

	public async Task<Result<PagedResponse<IReadOnlyList<ProductDto>>>> GetAllAsync(
			int pageNumber,
			int pageSize
	)
	{
		var totalCount = await context.Products.CountAsync();

		var products = await context
				.Products.AsNoTracking()
				.Skip((pageNumber - 1) * pageSize)
				.Take(pageSize)
				.Select(p => p.ToDto())
				.ToListAsync();

		return Result<PagedResponse<IReadOnlyList<ProductDto>>>.Success(
				new PagedResponse<IReadOnlyList<ProductDto>>(products, pageNumber, pageSize, totalCount)
		);
	}

	public async Task<Result<ProductDto>> CreateAsync(CreateProductRequest createProductRequest)
	{
		var product = createProductRequest.ToEntity();
		await context.Products.AddAsync(product);
		await context.SaveChangesAsync();
		return Result<ProductDto>.Success(product.ToDto());
	}

	public async Task<Result<ProductDto>> UpdateAsync(int id, UpdateProductRequest dto)
	{
		var existingProduct = await context.Products.FindAsync(id);
		if (existingProduct is null)
		{
			return Result<ProductDto>.Failure(
					$"Product with id {id} not found.",
					ErrorType.NotFound
			);
		}
		dto.UpdateEntity(existingProduct);
		await context.SaveChangesAsync();

		return Result<ProductDto>.Success(existingProduct.ToDto());
	}

	public async Task<Result> DeleteAsync(int id)
	{
		var existingProduct = await context.Products.FindAsync(id);
		if (existingProduct is null)
		{
			return Result.Failure($"Product with id {id} not found.", ErrorType.NotFound);
		}
		context.Products.Remove(existingProduct);
		await context.SaveChangesAsync();
		return Result.Success();
	}
}

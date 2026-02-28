using Ecommerce.Application.Dtos;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mime;

namespace Ecommerce.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Produces(MediaTypeNames.Application.Json)]
public class ProductsController(IProductService productService) : ControllerBase
{

	/// <summary>
	/// Retrieves a paginated list of products.
	/// </summary>
	[HttpGet]
	[ProducesResponseType(typeof(PagedResponse<IReadOnlyList<ProductDto>>), StatusCodes.Status200OK)]
	public async Task<ActionResult<PagedResponse<IReadOnlyList<ProductDto>>>> GetAll(
			[FromQuery] int pageNumber = 1,
			[FromQuery] int pageSize = 10)
	{
		// Ensure minimum values for pagination
		pageNumber = pageNumber < 1 ? 1 : pageNumber;
		pageSize = pageSize < 1 ? 10 : pageSize;

		var result = await productService.GetAllAsync(pageNumber, pageSize);
		return Ok(result);
	}

	/// <summary>
	/// Retrieves a specific product by its ID.
	/// </summary>
	[HttpGet("{id:int}")]
	[ProducesResponseType(typeof(ProductDto), StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	public async Task<ActionResult<ProductDto>> GetById(int id)
	{
		var product = await productService.GetByIdAsync(id);

		return product == null ? (ActionResult<ProductDto>)NotFound(new { Message = $"Product with ID {id} was not found." }) : (ActionResult<ProductDto>)Ok(product);
	}

	/// <summary>
	/// Creates a new product.
	/// </summary>
	[HttpPost]
	[ProducesResponseType(typeof(ProductDto), StatusCodes.Status201Created)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	public async Task<ActionResult<ProductDto>> Create([FromBody] CreateProductRequest request)
	{
		var createdProduct = await productService.CreateAsync(request);

		return CreatedAtAction(nameof(GetById), new { id = createdProduct.Id }, createdProduct);
	}

	/// <summary>
	/// Updates an existing product.
	/// </summary>
	[HttpPut("{id:int}")]
	[ProducesResponseType(typeof(ProductDto), StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	public async Task<ActionResult<ProductDto>> Update(int id, [FromBody] UpdateProductRequest request)
	{
		try
		{
			var updatedProduct = await productService.UpdateAsync(id, request);
			return Ok(updatedProduct);
		}
		catch (KeyNotFoundException ex)
		{
			return NotFound(new { ex.Message });
		}
	}

	/// <summary>
	/// Deletes a product from the catalog.
	/// </summary>
	[HttpDelete("{id:int}")]
	[ProducesResponseType(StatusCodes.Status204NoContent)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	public async Task<IActionResult> Delete(int id)
	{
		try
		{
			await productService.DeleteAsync(id);
			return NoContent(); // Standard 204 response for successful deletion
		}
		catch (KeyNotFoundException ex)
		{
			return NotFound(new { ex.Message });
		}
	}
}

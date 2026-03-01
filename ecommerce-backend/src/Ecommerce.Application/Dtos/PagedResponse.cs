namespace Ecommerce.Application.Dtos;

public sealed record PagedResponse<T>
(
	T Items,
	int PageNumber,
	int PageSize,
	int TotalItems
	)
{
	public int TotalPages => (int)Math.Ceiling((double)TotalItems / PageSize);
	public bool HasNextPage => PageNumber < TotalPages;
	public bool HasPreviousPage => PageNumber > 1;
}

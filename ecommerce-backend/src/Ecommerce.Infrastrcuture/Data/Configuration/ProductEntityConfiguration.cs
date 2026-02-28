using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ecommerce.Infrastrcuture.Data.Configuration;

public class ProductEntityConfiguration : IEntityTypeConfiguration<Product>
{
	public void Configure(EntityTypeBuilder<Product> builder)
	{
		builder.Property(p => p.Name)
			.IsRequired()
			.HasMaxLength(100);
		builder.Property(p => p.Description)
			.IsRequired()
			.HasMaxLength(500);
		builder.Property(p => p.Price)
			.IsRequired()
			.HasColumnType("decimal(8,2)");
		builder.Property(p => p.Quantity)
			.IsRequired();
	}
}

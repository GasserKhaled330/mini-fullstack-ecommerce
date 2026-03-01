namespace Ecommerce.Infrastructure.Data.Configuration;

public class OrderItemEntityConfiguration : IEntityTypeConfiguration<OrderItem>
{
	public void Configure(EntityTypeBuilder<OrderItem> builder)
	{
		builder.HasKey(oi => new { oi.OrderId, oi.ProductId });

		builder.Property(oi => oi.UnitPrice)
							 .IsRequired()
							 .HasColumnType("decimal(10,2)");
		builder.Property(oi => oi.Quantity)
							 .IsRequired();

		builder.HasOne(oi => oi.Product)
							 .WithMany()
							 .HasForeignKey(oi => oi.ProductId)
							 .OnDelete(DeleteBehavior.Restrict);
	}
}

namespace Ecommerce.Infrastructure.Data.Configuration;

public class OrderEntityConfiguration : IEntityTypeConfiguration<Order>
{
	public void Configure(EntityTypeBuilder<Order> builder)
	{
		builder.HasKey(o => o.Id);
		builder.Property(o => o.CustomerName).IsRequired().HasMaxLength(100);
		builder.Property(o => o.TotalAmount).IsRequired().HasColumnType("decimal(10,2)");
		builder.Property(o => o.DiscountAmount).IsRequired().HasColumnType("decimal(8,2)");
		builder.Property(o => o.CreatedAt)
			 .HasDefaultValueSql("GETUTCDATE()");

		builder.Property(o => o.TimeStamp)
			.IsRowVersion();

		builder.HasMany(o => o.OrderItems)
				 .WithOne(oi => oi.Order)
				 .HasForeignKey(oi => oi.OrderId)
				 .OnDelete(DeleteBehavior.Cascade);
	}
}
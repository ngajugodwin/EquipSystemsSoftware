using ItemBookingApp_API.Domain.Models.OrderAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ItemBookingApp_API.EntityConfiguration
{
    public class OrderConfiguration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.OwnsOne(x => x.ShipToAddress, a =>
            {
                a.WithOwner();
            });

            builder.OwnsOne(x => x.BookingInformation, a =>
            {
                a.WithOwner();
            });

            builder.Property(s => s.Status)
                .HasConversion(
                o => o.ToString(),
                o => (OrderStatus) Enum.Parse(typeof(OrderStatus), o)
                );

            builder.HasMany(x => x.OrderItems).WithOne().OnDelete(DeleteBehavior.ClientCascade);

            builder.Property(i => i.SubTotal)
                 .HasColumnType("decimal(18,2)");
        }
    }
}

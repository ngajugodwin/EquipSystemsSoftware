using ItemBookingApp_API.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ItemBookingApp_API.EntityConfiguration
{
    public class CustomerBasketConfiguration : IEntityTypeConfiguration<CustomerBasket>
    {
        public void Configure(EntityTypeBuilder<CustomerBasket> builder)
        {
            builder.HasKey(i => i.Id);

            builder.HasOne(b => b.User)
                .WithOne(x => x.CustomerBasket)
                .HasForeignKey<CustomerBasket>(x => x.UserId);

                        
            builder.HasMany(b => b.Items)
                .WithOne(b => b.CustomerBasket)
                .HasForeignKey(b => b.CustomerBasketId);

            builder.Property(i => i.ShippingPrice)
                    .HasPrecision(18, 2)
                   .IsRequired(false);

        }
    }
}

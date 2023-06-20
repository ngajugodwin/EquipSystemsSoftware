using ItemBookingApp_API.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ItemBookingApp_API.EntityConfiguration
{
    public class BasketItemConfiguration : IEntityTypeConfiguration<BasketItem>
    {
        public void Configure(EntityTypeBuilder<BasketItem> builder)
        {
            builder.HasKey(bi => new { bi.ItemId, bi.CustomerBasketId });



            builder.HasOne(bi => bi.Item)
                .WithMany(bi => bi.BasketItems)
                .HasForeignKey(bi => bi.ItemId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(bi => bi.CustomerBasket)
                .WithMany(bi => bi.Items)
                .HasForeignKey(bi => bi.CustomerBasketId)
                .HasPrincipalKey(bi => bi.Id)
                .OnDelete(DeleteBehavior.Cascade);




        }
    }
}

using ItemBookingApp_API.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ItemBookingApp_API.EntityConfiguration
{
    public class ItemConfiguration : IEntityTypeConfiguration<Item>
    {
        public void Configure(EntityTypeBuilder<Item> builder)
        {
            builder.HasKey(i => i.Id);

            builder.Property(i => i.Name)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(i => i.Price)
                .HasPrecision(18, 2)
               .IsRequired();

            builder.Property(i => i.ItemState)
                .HasDefaultValue(ItemState.Available);

            builder.Property(i => i.IsActive)
                .HasDefaultValue(true);

            builder.Property(i => i.SerialNumber)
                .IsRequired();

            builder.Property(i => i.Description)
             .IsRequired();

            builder.HasOne(b => b.ItemType)
             .WithMany(i => i.Items)
             .HasForeignKey(b => b.ItemTypeId)
             .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(b => b.Category)         
            .WithMany(i => i.Items)
            .HasForeignKey(b => b.CategoryId)        
            .OnDelete(DeleteBehavior.Restrict);
        }
    }
}

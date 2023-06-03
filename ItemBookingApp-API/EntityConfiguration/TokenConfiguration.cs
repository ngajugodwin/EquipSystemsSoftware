using ItemBookingApp_API.Domain.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace ItemBookingApp_API.EntityConfiguration
{
    public class TokenConfiguration : IEntityTypeConfiguration<Token>

    {
        public void Configure(EntityTypeBuilder<Token> builder)
        {
            builder.HasKey(t => t.Id);

            builder.Property(t => t.ClientId)
             .IsRequired();

            builder.Property(t => t.Value)
                .IsRequired();

            builder.HasOne(t => t.User)
                .WithMany(u => u.Tokens)
                .HasForeignKey(t => t.UserId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}

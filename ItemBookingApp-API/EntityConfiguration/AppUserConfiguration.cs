using ItemBookingApp_API.Domain.Models;
using ItemBookingApp_API.Domain.Models.Identity;
using Microsoft.EntityFrameworkCore;

namespace ItemBookingApp_API.EntityConfiguration
{
    public class AppUserConfiguration : IEntityTypeConfiguration<AppUser>
    {
        public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<AppUser> builder)
        {
            builder.HasKey(u => u.Id);

            builder.Property(u => u.Status)
                .HasDefaultValue(EntityStatus.Pending);

            builder.Property(u => u.FirstName)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(u => u.LastName)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(u => u.Email)
               .IsRequired();

            builder.HasOne(u => u.Organisation)
             .WithMany(o => o.Users)             
             .HasForeignKey(u => u.OrganisationId)
             .IsRequired(false)
             .OnDelete(DeleteBehavior.Restrict);
        }
    }
}

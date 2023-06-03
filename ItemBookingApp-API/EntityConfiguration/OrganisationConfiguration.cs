using ItemBookingApp_API.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ItemBookingApp_API.EntityConfiguration
{
    public class OrganisationConfiguration : IEntityTypeConfiguration<Organisation>
    {
        public void Configure(EntityTypeBuilder<Organisation> builder)
        {

            builder.HasKey(i => i.Id);

            builder.Property(i => i.Name)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(i => i.RegistrationNumber)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(i => i.Address)
                .IsRequired();

            builder.Property(i => i.DateOfIncorporation)
                .IsRequired();
        }
    }
}

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using YesLocation.Domain.Entities;

namespace YesLocation.Infrastructure.Configurations
{
  internal class LocationConfiguration : IEntityTypeConfiguration<Location>
  {
    public void Configure(EntityTypeBuilder<Location> builder)
    {
      builder.Property(l => l.Name)
          .HasMaxLength(100);

      builder.Property(l => l.Address)
          .HasMaxLength(200);

      builder.Property(l => l.City)
          .HasMaxLength(100);

      builder.Property(l => l.PostalCode)
          .HasMaxLength(20);

      builder.HasMany(l => l.PickupBookings)
          .WithOne(b => b.PickupLocation)
          .HasForeignKey(b => b.PickupLocationId);

      builder.HasMany(l => l.ReturnBookings)
          .WithOne(b => b.ReturnLocation)
          .HasForeignKey(b => b.ReturnLocationId);
    }
  }
}

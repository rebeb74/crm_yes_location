using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using YesLocation.Domain.Entities;

namespace YesLocation.Infrastructure.Configurations
{
  internal class BookingConfiguration : IEntityTypeConfiguration<Booking>
  {
    public void Configure(EntityTypeBuilder<Booking> builder)
    {
      builder.HasOne(b => b.Customer)
          .WithMany()
          .HasForeignKey(b => b.CustomerId)
          .OnDelete(DeleteBehavior.Restrict);

      builder.HasOne(b => b.Vehicle)
          .WithMany(v => v.Bookings)
          .HasForeignKey(b => b.VehicleId)
          .OnDelete(DeleteBehavior.Restrict);

      builder.HasOne(b => b.PickupLocation)
          .WithMany(l => l.PickupBookings)
          .HasForeignKey(b => b.PickupLocationId)
          .OnDelete(DeleteBehavior.Restrict);

      builder.HasOne(b => b.ReturnLocation)
          .WithMany(l => l.ReturnBookings)
          .HasForeignKey(b => b.ReturnLocationId)
          .OnDelete(DeleteBehavior.Restrict);

      builder.Property(b => b.TotalAmount)
          .HasPrecision(18, 2);

      builder.Property(b => b.Notes)
          .HasMaxLength(500);
    }
  }
}

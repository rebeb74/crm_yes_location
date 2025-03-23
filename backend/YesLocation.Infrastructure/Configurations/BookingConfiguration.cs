using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using YesLocation.Domain.Entities;

namespace YesLocation.Infrastructure.Configurations
{
    internal class BookingConfiguration : IEntityTypeConfiguration<Booking>
    {
        public void Configure(EntityTypeBuilder<Booking> builder)
        {
            builder.Property(b => b.TotalAmount)
                .HasPrecision(7, 2);

            builder.Property(b => b.Notes)
                .HasMaxLength(500);

            builder.HasOne(b => b.Customer)
                .WithMany()
                .HasForeignKey(b => b.CustomerId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(b => b.Vehicle)
                .WithMany()
                .HasForeignKey(b => b.VehicleId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(b => b.PickupLocation)
                .WithMany()
                .HasForeignKey(b => b.PickupLocationId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(b => b.ReturnLocation)
                .WithMany()
                .HasForeignKey(b => b.ReturnLocationId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(b => b.Quotation)
                .WithMany()
                .HasForeignKey(b => b.QuotationId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(b => b.Invoice)
                .WithMany()
                .HasForeignKey(b => b.InvoiceId)
                .OnDelete(DeleteBehavior.Restrict);

        }
    }
}

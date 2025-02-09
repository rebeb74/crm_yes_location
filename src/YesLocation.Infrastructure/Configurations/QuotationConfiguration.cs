using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using YesLocation.Domain.Entities;

namespace YesLocation.Infrastructure.Configurations
{
  internal class QuotationConfiguration : IEntityTypeConfiguration<Quotation>
  {
    public void Configure(EntityTypeBuilder<Quotation> builder)
    {
      builder.HasOne(q => q.Customer)
          .WithMany(c => c.Quotations)
          .HasForeignKey(q => q.CustomerId)
          .OnDelete(DeleteBehavior.Restrict);

      builder.HasOne(q => q.Vehicle)
          .WithMany()
          .HasForeignKey(q => q.VehicleId)
          .OnDelete(DeleteBehavior.Restrict);

      // Configuration de la relation one-to-one avec Booking
      builder.HasOne(q => q.Booking)
          .WithOne(b => b.Quotation)
          .HasForeignKey<Booking>(b => b.QuotationId)
          .OnDelete(DeleteBehavior.Restrict);

      builder.Property(q => q.Amount)
          .HasPrecision(18, 2);
    }
  }
}

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using YesLocation.Domain.Entities;

namespace YesLocation.Infrastructure.Configurations
{
  internal class QuotationConfiguration : IEntityTypeConfiguration<Quotation>
  {
    public void Configure(EntityTypeBuilder<Quotation> builder)
    {
      builder.Property(q => q.Amount)
          .HasPrecision(7, 2);

      // Relationships
      builder.HasOne(q => q.Customer)
          .WithMany(c => c.Quotations)
          .HasForeignKey(q => q.CustomerId)
          .OnDelete(DeleteBehavior.Restrict);

      builder.HasOne(q => q.Vehicle)
          .WithMany()
          .HasForeignKey(q => q.VehicleId)
          .OnDelete(DeleteBehavior.Restrict);

      builder.HasOne(q => q.Booking)
          .WithOne(b => b.Quotation)
          .HasForeignKey<Booking>(b => b.QuotationId)
          .OnDelete(DeleteBehavior.Restrict);
    }
  }
}

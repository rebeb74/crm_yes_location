using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using YesLocation.Domain.Entities;

namespace YesLocation.Infrastructure.Configurations
{
  internal class InvoiceConfiguration : IEntityTypeConfiguration<Invoice>
  {
    public void Configure(EntityTypeBuilder<Invoice> builder)
    {
      builder.Property(i => i.InvoiceNumber)
          .HasMaxLength(50);

      builder.Property(i => i.Amount)
          .HasPrecision(7, 2);

      builder.Property(i => i.PaidAmount)
          .HasPrecision(7, 2);

      // Relationships
      builder.HasOne(i => i.Booking)
          .WithOne(b => b.Invoice)
          .HasForeignKey<Booking>(b => b.InvoiceId)
          .OnDelete(DeleteBehavior.Restrict);

      builder.HasMany(i => i.Payments)
          .WithOne(p => p.Invoice)
          .HasForeignKey(p => p.InvoiceId)
          .OnDelete(DeleteBehavior.Cascade);
    }
  }
}

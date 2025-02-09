using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using YesLocation.Domain.Entities;

namespace YesLocation.Infrastructure.Configurations
{
  internal class PaymentConfiguration : IEntityTypeConfiguration<Payment>
  {
    public void Configure(EntityTypeBuilder<Payment> builder)
    {
      builder.HasOne(p => p.Invoice)
          .WithMany(i => i.Payments)
          .HasForeignKey(p => p.InvoiceId)
          .OnDelete(DeleteBehavior.Cascade);

      builder.Property(p => p.Amount)
          .HasPrecision(18, 2);

      builder.Property(p => p.TransactionReference)
          .HasMaxLength(100);
    }
  }
}

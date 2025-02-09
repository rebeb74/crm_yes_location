using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using YesLocation.Domain.Entities;

namespace YesLocation.Infrastructure.Configurations
{
  internal class VehicleConfiguration : IEntityTypeConfiguration<Vehicle>
  {
    public void Configure(EntityTypeBuilder<Vehicle> builder)
    {
      builder.Property(v => v.Brand)
          .IsRequired()
          .HasMaxLength(50);

      builder.Property(v => v.Model)
          .IsRequired()
          .HasMaxLength(50);

      builder.Property(v => v.RegistrationNumber)
          .IsRequired()
          .HasMaxLength(20);

      builder.Property(v => v.DailyRate)
          .HasPrecision(18, 2);

      builder.Property(v => v.FuelType)
          .HasMaxLength(30);

      builder.Property(v => v.Transmission)
          .HasMaxLength(30);
    }
  }
}

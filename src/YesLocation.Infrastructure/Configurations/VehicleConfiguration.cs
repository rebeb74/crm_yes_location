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
                .HasMaxLength(50);

            builder.Property(v => v.Model)
                .HasMaxLength(50);

            builder.Property(v => v.RegistrationNumber)
                .HasMaxLength(20);

            builder.Property(v => v.DailyRate)
                .HasPrecision(7, 2);

            builder.Property(v => v.FuelType)
                .HasMaxLength(30);

            builder.Property(v => v.Transmission)
                .HasMaxLength(30);
        }
    }
}

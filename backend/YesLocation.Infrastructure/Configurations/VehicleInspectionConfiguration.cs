using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using YesLocation.Domain.Entities;

namespace YesLocation.Infrastructure.Configurations;

public class VehicleInspectionConfiguration : IEntityTypeConfiguration<VehicleInspection>
{
  public void Configure(EntityTypeBuilder<VehicleInspection> builder)
  {
    builder.ToTable("VehicleInspections");

    builder.HasKey(x => x.Id);

    builder.Property(x => x.Type)
        .IsRequired();

    builder.Property(x => x.InspectionDate)
        .IsRequired();

    builder.Property(x => x.Notes)
        .HasMaxLength(1000);

    builder.Property(x => x.SignatureUrl)
        .HasMaxLength(500);

    // Nouvelles propriétés
    builder.Property(x => x.Mileage)
        .IsRequired();

    builder.Property(x => x.VehicleCleaned)
        .IsRequired();

    builder.Property(x => x.FuelLevel)
        .IsRequired();

    // Relations
    builder.HasOne(x => x.Booking)
        .WithMany(b => b.VehicleInspections)
        .HasForeignKey(x => x.BookingId)
        .OnDelete(DeleteBehavior.Cascade);

    builder.HasOne(x => x.InspectedByUser)
        .WithMany()
        .HasForeignKey(x => x.InspectedByUserId)
        .OnDelete(DeleteBehavior.SetNull);

    builder.HasMany(x => x.Incidents)
        .WithOne(i => i.VehicleInspection)
        .HasForeignKey(i => i.VehicleInspectionId)
        .OnDelete(DeleteBehavior.Cascade);
  }
}
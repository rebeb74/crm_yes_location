using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using YesLocation.Domain.Entities;

namespace YesLocation.Infrastructure.Configurations
{
  internal class VehiclePricingConfiguration : IEntityTypeConfiguration<VehiclePricing>
  {
    public void Configure(EntityTypeBuilder<VehiclePricing> builder)
    {
      builder.Property(vp => vp.DailyRate)
          .HasPrecision(7, 2);

      // Chaque combinaison véhicule/saison/durée doit être unique
      builder.HasIndex(vp => new { vp.VehicleId, vp.SeasonId, vp.DurationTierId })
          .IsUnique();

      // Relations
      builder.HasOne(vp => vp.Vehicle)
          .WithMany(v => v.Pricings)
          .HasForeignKey(vp => vp.VehicleId)
          .OnDelete(DeleteBehavior.Cascade);

      builder.HasOne(vp => vp.Season)
          .WithMany(s => s.VehiclePricings)
          .HasForeignKey(vp => vp.SeasonId)
          .OnDelete(DeleteBehavior.Cascade);

      builder.HasOne(vp => vp.DurationTier)
          .WithMany(dt => dt.VehiclePricings)
          .HasForeignKey(vp => vp.DurationTierId)
          .OnDelete(DeleteBehavior.Cascade);
    }
  }
}
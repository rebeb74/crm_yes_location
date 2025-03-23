using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using YesLocation.Domain.Entities;

namespace YesLocation.Infrastructure.Configurations
{
  internal class MaintenanceRecordConfiguration : IEntityTypeConfiguration<MaintenanceRecord>
  {
    public void Configure(EntityTypeBuilder<MaintenanceRecord> builder)
    {
      builder.Property(m => m.Description)
          .HasMaxLength(500);

      builder.Property(m => m.Cost)
          .HasPrecision(7, 2);

      builder.Property(m => m.ServiceProvider)
          .HasMaxLength(100);

      // Relationships
      builder.HasOne(m => m.Vehicle)
          .WithMany(v => v.MaintenanceRecords)
          .HasForeignKey(m => m.VehicleId)
          .OnDelete(DeleteBehavior.Cascade);
    }
  }
}

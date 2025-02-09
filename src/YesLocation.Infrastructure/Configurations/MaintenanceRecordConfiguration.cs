using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using YesLocation.Domain.Entities;

namespace YesLocation.Infrastructure.Configurations
{
  internal class MaintenanceRecordConfiguration : IEntityTypeConfiguration<MaintenanceRecord>
  {
    public void Configure(EntityTypeBuilder<MaintenanceRecord> builder)
    {
      builder.HasOne(m => m.Vehicle)
          .WithMany(v => v.MaintenanceRecords)
          .HasForeignKey(m => m.VehicleId)
          .OnDelete(DeleteBehavior.Cascade);

      builder.Property(m => m.Description)
          .IsRequired()
          .HasMaxLength(500);

      builder.Property(m => m.Cost)
          .HasPrecision(18, 2);

      builder.Property(m => m.ServiceProvider)
          .HasMaxLength(100);
    }
  }
}

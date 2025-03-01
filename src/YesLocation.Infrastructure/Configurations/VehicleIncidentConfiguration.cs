using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using YesLocation.Domain.Entities;

namespace YesLocation.Infrastructure.Configurations;

public class VehicleIncidentConfiguration : IEntityTypeConfiguration<VehicleIncident>
{
    public void Configure(EntityTypeBuilder<VehicleIncident> builder)
    {
        builder.ToTable("VehicleIncidents");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.PartLocation)
            .IsRequired();

        builder.Property(x => x.Type)
            .IsRequired();

        builder.Property(x => x.Description)
            .HasMaxLength(1000);

        // Relations
        builder.HasOne(x => x.VehicleInspection)
            .WithMany(i => i.Incidents)
            .HasForeignKey(x => x.VehicleInspectionId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(x => x.Photos)
            .WithOne(p => p.VehicleIncident)
            .HasForeignKey(p => p.VehicleIncidentId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
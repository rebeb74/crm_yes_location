using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using YesLocation.Domain.Entities;

namespace YesLocation.Infrastructure.Configurations;

public class VehicleIncidentPhotoConfiguration : IEntityTypeConfiguration<VehicleIncidentPhoto>
{
  public void Configure(EntityTypeBuilder<VehicleIncidentPhoto> builder)
  {
    builder.ToTable("VehicleIncidentPhotos");

    builder.HasKey(x => x.Id);

    builder.Property(x => x.PhotoUrl)
        .IsRequired()
        .HasMaxLength(500);

    builder.Property(x => x.Caption)
        .HasMaxLength(255);

    // Relations
    builder.HasOne(x => x.VehicleIncident)
        .WithMany(i => i.Photos)
        .HasForeignKey(x => x.VehicleIncidentId)
        .OnDelete(DeleteBehavior.Cascade);
  }
}
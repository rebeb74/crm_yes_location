using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using YesLocation.Domain.Entities;

namespace YesLocation.Infrastructure.Configurations
{
  internal class LocationConfiguration : IEntityTypeConfiguration<Location>
  {
    public void Configure(EntityTypeBuilder<Location> builder)
    {
      builder.Property(l => l.Name)
          .IsRequired()
          .HasMaxLength(100);

      builder.Property(l => l.Address)
          .IsRequired()
          .HasMaxLength(200);

      builder.Property(l => l.City)
          .IsRequired()
          .HasMaxLength(100);

      builder.Property(l => l.PostalCode)
          .IsRequired()
          .HasMaxLength(20);

      builder.Property(l => l.Country)
          .IsRequired()
          .HasMaxLength(100);
    }
  }
}

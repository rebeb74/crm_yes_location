using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using YesLocation.Domain.Entities;

namespace YesLocation.Infrastructure.Configurations
{
  internal class DurationTierConfiguration : IEntityTypeConfiguration<DurationTier>
  {
    public void Configure(EntityTypeBuilder<DurationTier> builder)
    {
      builder.Property(dt => dt.Name)
          .HasMaxLength(50);

      // Un palier est unique par MinDays-MaxDays
      builder.HasIndex(dt => new { dt.MinDays, dt.MaxDays })
          .IsUnique();

      // Contrainte : MaxDays doit être >= MinDays ou NULL
      builder.ToTable(t => t.HasCheckConstraint("CK_DurationTier_DayRange",
          "MaxDays IS NULL OR MaxDays >= MinDays"));
    }
  }
}
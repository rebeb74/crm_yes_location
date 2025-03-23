using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using YesLocation.Domain.Entities;

namespace YesLocation.Infrastructure.Configurations
{
  internal class SeasonConfiguration : IEntityTypeConfiguration<Season>
  {
    public void Configure(EntityTypeBuilder<Season> builder)
    {
      builder.Property(s => s.Name)
          .HasMaxLength(50);

      builder.HasIndex(s => new { s.StartDate, s.EndDate, s.Year })
          .IsUnique();

      // Contrainte : EndDate doit être >= StartDate
      builder.ToTable(t => t.HasCheckConstraint("CK_Season_DateRange", "EndDate >= StartDate"));
    }
  }
}
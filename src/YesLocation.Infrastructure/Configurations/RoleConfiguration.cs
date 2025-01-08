using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using YesLocation.Domain.Entities;

namespace YesLocation.Infrastructure.Configurations;

public class RoleConfiguration : IEntityTypeConfiguration<Role>
{
  public void Configure(EntityTypeBuilder<Role> builder)
  {
    builder.Property(e => e.Name)
        .IsRequired()
        .HasMaxLength(50);

    builder.Property(e => e.Description)
        .HasMaxLength(200);

    builder.Property(e => e.Value)
        .IsRequired();

    builder.HasIndex(e => e.Name)
        .IsUnique();

    builder.HasIndex(e => e.Value)
        .IsUnique();
  }

}

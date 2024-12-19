using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using YesLocation.Domain.Entities;

namespace YesLocation.Infrastructure.Configurations
{
  internal class UserConfiguration : IEntityTypeConfiguration<User>
  {
    public void Configure(EntityTypeBuilder<User> builder)
    {
      builder.HasIndex(e => e.Username)
          .IsUnique();

      builder.Property(e => e.Username)
          .IsRequired()
          .HasMaxLength(50);

      builder.Property(e => e.FirstName)
          .HasMaxLength(50);

      builder.Property(e => e.LastName)
          .HasMaxLength(50);

      builder.HasIndex(e => e.Email)
          .IsUnique();

      builder.Property(e => e.Email)
          .HasMaxLength(50)
          .IsRequired();

      builder.HasData(
              new User
              {
                Id = 1,
                Username = "admin",
                FirstName = "Admin",
                LastName = "Admin",
                Email = "bertrandpetit10@gmail.com",
              }

          );
    }
  }
}

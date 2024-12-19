using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using YesLocation.Domain.Entities;

namespace YesLocation.Infrastructure.Configurations
{
    internal class AuthConfiguration : IEntityTypeConfiguration<Auth>
    {
        public void Configure(EntityTypeBuilder<Auth> builder)
        {

            builder.HasIndex(e => e.Username)
                .IsUnique();
            builder.Property(e => e.Username)
                .HasMaxLength(50)
                .IsRequired();

            builder.HasIndex(e => e.Email)
                .IsUnique();
            builder.Property(e => e.Email)
                .HasMaxLength(50)
                .IsRequired();

            builder.Property(e => e.PasswordHash)
                .HasColumnType("tinyblob")
                .IsRequired();

            builder.Property(e => e.PasswordSalt)
                .HasColumnType("tinyblob")
                .IsRequired();
        }
    }
}

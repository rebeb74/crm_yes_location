using System.Security.Cryptography;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.Extensions.Configuration;
using YesLocation.Domain.Entities;
using YesLocation.Domain.Interfaces;

namespace YesLocation.Infrastructure.Configurations
{
    internal class AuthConfiguration : IEntityTypeConfiguration<Auth>
    {
        public void Configure(EntityTypeBuilder<Auth> builder)
        {
            builder.Property(e => e.PasswordHash)
                .HasColumnType("tinyblob")
                .IsRequired();

            builder.Property(e => e.PasswordSalt)
                .HasColumnType("tinyblob")
                .IsRequired();

            builder.HasOne(a => a.User)
                .WithOne(u => u.Auth)
                .HasForeignKey<Auth>(a => a.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}

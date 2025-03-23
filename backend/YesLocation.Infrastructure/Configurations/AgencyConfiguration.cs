using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using YesLocation.Domain.Entities;

namespace YesLocation.Infrastructure.Configurations
{
    internal class AgencyConfiguration : IEntityTypeConfiguration<Agency>
    {
        public void Configure(EntityTypeBuilder<Agency> builder)
        {
            builder.Property(a => a.CompanyName)
                .HasMaxLength(100);

            builder.Property(a => a.SiretNumber)
                .HasMaxLength(20);

            builder.Property(a => a.VatNumber)
                .HasMaxLength(30);

            builder.Property(a => a.PhoneNumber)
                .HasMaxLength(20);

            builder.Property(a => a.Email)
                .HasMaxLength(100);

            builder.Property(a => a.Website)
                .HasMaxLength(200);

            // Address
            builder.Property(a => a.StreetAddress)
                .HasMaxLength(200);

            builder.Property(a => a.PostalCode)
                .HasMaxLength(10);

            builder.Property(a => a.City)
                .HasMaxLength(100);

            // Other information
            builder.Property(a => a.LogoUrl)
                .HasMaxLength(500);

            builder.Property(a => a.BankAccountIban)
                .HasMaxLength(50);

            builder.Property(a => a.BankAccountBic)
                .HasMaxLength(20);

            // Propriétés de thème
            builder.Property(a => a.PrimaryColor)
                .HasMaxLength(50);
            builder.Property(a => a.SecondaryColor)
                .HasMaxLength(50);
            builder.Property(a => a.AccentColor)
                .HasMaxLength(50);
            builder.Property(a => a.SuccessColor)
                .HasMaxLength(50);
            builder.Property(a => a.WarningColor)
                .HasMaxLength(50);
            builder.Property(a => a.ErrorColor)
                .HasMaxLength(50);

            // Mode sombre
            builder.Property(a => a.UseDarkMode)
                .HasDefaultValue(false);
            builder.Property(a => a.DarkModePrimaryColor)
                .HasMaxLength(50);
            builder.Property(a => a.DarkModeSecondaryColor)
                .HasMaxLength(50);
            builder.Property(a => a.DarkModeAccentColor)
                .HasMaxLength(50);
            builder.Property(a => a.DarkModeSuccessColor)
                .HasMaxLength(50);
            builder.Property(a => a.DarkModeWarningColor)
                .HasMaxLength(50);
            builder.Property(a => a.DarkModeErrorColor)
                .HasMaxLength(50);

            // Ensure there can be only one agency record
            builder.HasIndex(a => a.Id)
                .HasDatabaseName("IX_AgencySingleRecord")
                .IsUnique();
        }
    }
}
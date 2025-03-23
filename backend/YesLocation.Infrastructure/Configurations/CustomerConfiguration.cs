using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using YesLocation.Domain.Entities;

namespace YesLocation.Infrastructure.Configurations
{
    internal class CustomerConfiguration : IEntityTypeConfiguration<Customer>
    {
        public void Configure(EntityTypeBuilder<Customer> builder)
        {
            builder.Property(c => c.FirstName)
                .HasMaxLength(50);

            builder.Property(c => c.LastName)
                .HasMaxLength(50);

            builder.Property(c => c.Email)
                .HasMaxLength(100);

            builder.Property(c => c.Phone)
                .HasMaxLength(20);

            builder.Property(c => c.Address)
                .HasMaxLength(200);

            builder.Property(c => c.City)
                .HasMaxLength(100);

            builder.Property(c => c.State)
                .HasMaxLength(50);

            builder.Property(c => c.Zip)
                .HasMaxLength(10);

            builder.Property(c => c.Country)
                .HasMaxLength(100);

            builder.Property(c => c.Notes)
                .HasMaxLength(500);

            builder.Property(c => c.CompanyName)
                .HasMaxLength(100);

            builder.Property(c => c.VatNumber)
                .HasMaxLength(50);

            builder.Property(c => c.DriverLicenseNumber)
                .HasMaxLength(50);

            builder.Property(c => c.EmergencyContactName)
                .HasMaxLength(100);

            builder.Property(c => c.EmergencyContactPhone)
                .HasMaxLength(20);

            // Relationships
            builder.HasMany(c => c.Bookings)
                .WithOne(b => b.Customer)
                .HasForeignKey(b => b.CustomerId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(c => c.Quotations)
                .WithOne(q => q.Customer)
                .HasForeignKey(q => q.CustomerId)
                .OnDelete(DeleteBehavior.Restrict);

        }
    }
}

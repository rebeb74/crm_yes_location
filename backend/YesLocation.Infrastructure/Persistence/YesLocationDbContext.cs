using System.Reflection;
using Microsoft.EntityFrameworkCore;
using YesLocation.Domain.Entities;
using YesLocation.Domain.Interfaces;
using YesLocation.Infrastructure.Extensions;

namespace YesLocation.Infrastructure.Persistence;

public class YesLocationDbContext : DbContext
{
    private readonly ICurrentUserService _currentUserService;

    // Entités d'authentification et utilisateurs
    public virtual DbSet<User> Users { get; set; } = null!;
    public virtual DbSet<Auth> Auth { get; set; } = null!;
    public virtual DbSet<Role> Roles { get; set; } = null!;
    public virtual DbSet<UserRole> UserRoles { get; set; } = null!;

    // Entités principales de gestion de location
    public virtual DbSet<Booking> Bookings { get; set; } = null!;
    public virtual DbSet<Vehicle> Vehicles { get; set; } = null!;
    public virtual DbSet<Customer> Customers { get; set; } = null!;
    public virtual DbSet<Location> Locations { get; set; } = null!;
    public virtual DbSet<Quotation> Quotations { get; set; } = null!;
    public virtual DbSet<Invoice> Invoices { get; set; } = null!;
    public virtual DbSet<Payment> Payments { get; set; } = null!;
    public virtual DbSet<MaintenanceRecord> MaintenanceRecords { get; set; } = null!;
    public virtual DbSet<Agency> Agencies { get; set; } = null!;

    // Entités pour le système de tarification
    public virtual DbSet<Season> Seasons { get; set; } = null!;
    public virtual DbSet<DurationTier> DurationTiers { get; set; } = null!;
    public virtual DbSet<VehiclePricing> VehiclePricings { get; set; } = null!;

    // Entités pour le système d'état des lieux
    public virtual DbSet<VehicleInspection> VehicleInspections { get; set; } = null!;
    public virtual DbSet<VehicleIncident> VehicleIncidents { get; set; } = null!;
    public virtual DbSet<VehicleIncidentPhoto> VehicleIncidentPhotos { get; set; } = null!;

    public YesLocationDbContext(DbContextOptions<YesLocationDbContext> options, ICurrentUserService currentUserService) : base(options)
    {
        _currentUserService = currentUserService;
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        this.UpdatePaidAmounts();
        var entries = ChangeTracker.Entries<BaseModel>().Where(q => q.State == EntityState.Modified || q.State == EntityState.Added);

        foreach (var entry in entries)
        {
            if (entry.State == EntityState.Added)
            {
                entry.Entity.CreatedAt = DateTime.UtcNow;
                entry.Entity.CreatedBy = _currentUserService!.Id;
            }
            entry.Entity.UpdatedAt = DateTime.UtcNow;
            entry.Entity.UpdatedBy = _currentUserService!.Id;
        }

        return base.SaveChangesAsync(cancellationToken);
    }

    public override int SaveChanges()
    {
        this.UpdatePaidAmounts();
        var entries = ChangeTracker.Entries<BaseModel>().Where(q => q.State == EntityState.Modified || q.State == EntityState.Added);

        foreach (var entry in entries)
        {
            if (entry.State == EntityState.Added)
            {
                entry.Entity.CreatedAt = DateTime.UtcNow;
                entry.Entity.CreatedBy = _currentUserService!.Id;
            }
            entry.Entity.UpdatedAt = DateTime.UtcNow;
            entry.Entity.UpdatedBy = _currentUserService!.Id;
        }

        return base.SaveChanges();
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>().ToTable("Users");
        modelBuilder.Entity<Role>().ToTable("Roles");
        modelBuilder.Entity<UserRole>().ToTable("UserRoles");
        modelBuilder.Entity<Auth>().ToTable("Auth");
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}
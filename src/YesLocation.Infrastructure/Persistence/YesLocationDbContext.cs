using System.Reflection;
using Microsoft.EntityFrameworkCore;
using YesLocation.Domain.Entities;
using YesLocation.Domain.Interfaces;

namespace YesLocation.Infrastructure.Persistence;

public class YesLocationDbContext : DbContext
{
    private readonly ICurrentUserService _currentUserService;

    public virtual DbSet<User> Users { get; set; } = null!;
    public virtual DbSet<Auth> Auth { get; set; } = null!;

    public YesLocationDbContext(DbContextOptions<YesLocationDbContext> options, ICurrentUserService currentUserService) : base(options)
    {
        _currentUserService = currentUserService;
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
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
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}
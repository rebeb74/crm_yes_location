using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using YesLocation.Domain.Entities;
using YesLocation.Domain.Interfaces;

namespace YesLocation.Infrastructure.Persistence;

public class YesLocationDbContext : DbContext
{
    private readonly IConfiguration _configuration;
    private readonly IEnvironmentService _env;
    public YesLocationDbContext(IConfiguration configuration, IEnvironmentService env)
    {
        _configuration = configuration;
        _env = env;
        Users = Set<User>();
        Auth = Set<Auth>();
    }
    // public DbSet<Client> Clients { get; set; }
    // public DbSet<Voiture> Voitures { get; set; }
    public virtual DbSet<User> Users { get; set; }
    public virtual DbSet<Auth> Auth { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseMySql(_configuration.GetConnectionString("DefaultConnection"),
                          new MySqlServerVersion(new Version(8, 0, 36)),
                          mySqlOptions => mySqlOptions.EnableRetryOnFailure(
                                            maxRetryCount: 10,
                                            maxRetryDelay: TimeSpan.FromSeconds(30),
                                            errorNumbersToAdd: null)
                          );

            if (_env.IsDevelopment())
            {
                optionsBuilder
                    .LogTo(Console.WriteLine, LogLevel.Information)
                    .EnableSensitiveDataLogging()
                    .EnableDetailedErrors();
            }
        }
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        var entries = ChangeTracker.Entries<BaseModel>().Where(q => q.State == EntityState.Modified || q.State == EntityState.Added);

        foreach (var entry in entries)
        {
            entry.Entity.CreatedAt = DateTime.UtcNow;
            entry.Entity.CreatedBy = "System";

            if (entry.State == EntityState.Added)
            {
                entry.Entity.UpdatedAt = DateTime.UtcNow;
                entry.Entity.CreatedBy = "System";
            }
        }

        return base.SaveChangesAsync(cancellationToken);
    }

    public override int SaveChanges()
    {
        var entries = ChangeTracker.Entries<BaseModel>().Where(q => q.State == EntityState.Modified || q.State == EntityState.Added);

        foreach (var entry in entries)
        {
            entry.Entity.CreatedAt = DateTime.UtcNow;
            entry.Entity.CreatedBy = "System";

            if (entry.State == EntityState.Added)
            {
                entry.Entity.UpdatedAt = DateTime.UtcNow;
                entry.Entity.CreatedBy = "System";
            }
        }

        return base.SaveChanges();
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>().ToTable("Users");
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}
using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using YesLocation.Domain.Entities;
using YesLocation.Domain.Interfaces;

namespace YesLocation.Infrastructure.Persistence;

public class YesLocationDbContext : DbContext
{
    private readonly IConfiguration? _configuration;
    private readonly IEnvironmentService? _env;
    private readonly ICurrentUserService _currentUserService;

    public virtual DbSet<User> Users { get; set; } = null!;
    public virtual DbSet<Auth> Auth { get; set; } = null!;

    // 1. Constructeur habituel pour l’application
    public YesLocationDbContext(IConfiguration configuration, IEnvironmentService env, ICurrentUserService currentUserService)
    {
        _configuration = configuration;
        _env = env;
        _currentUserService = currentUserService;
        Users = Set<User>();
        Auth = Set<Auth>();
    }

    // 2. Constructeur optionnel pour les tests ou scénarios InMemory
    public YesLocationDbContext(DbContextOptions<YesLocationDbContext> options, ICurrentUserService currentUserService)
        : base(options)
    {
        _currentUserService = currentUserService;
        Users = Set<User>();
        Auth = Set<Auth>();
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseMySql(_configuration!.GetConnectionString("DefaultConnection"),
                          new MySqlServerVersion(new Version(8, 0, 36)),
                          mySqlOptions => mySqlOptions.EnableRetryOnFailure(
                                            maxRetryCount: 10,
                                            maxRetryDelay: TimeSpan.FromSeconds(30),
                                            errorNumbersToAdd: null)
                          );

            if (_env!.IsDevelopment())
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
            if (entry.State == EntityState.Added)
            {
                entry.Entity.CreatedAt = DateTime.UtcNow;
                entry.Entity.CreatedBy = _currentUserService.Id;
            }
            entry.Entity.UpdatedAt = DateTime.UtcNow;
            entry.Entity.UpdatedBy = _currentUserService.Id;
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
                entry.Entity.CreatedBy = _currentUserService.Id;
            }
            entry.Entity.UpdatedAt = DateTime.UtcNow;
            entry.Entity.UpdatedBy = _currentUserService.Id;
        }

        return base.SaveChanges();
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>().ToTable("Users");
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}
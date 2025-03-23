using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using YesLocation.Domain.Entities;
using YesLocation.Domain.Interfaces;
using YesLocation.Infrastructure.Persistence;

namespace YesLocation.Infrastructure.Seed
{
  public static class DatabaseSeeder
  {
    public static async Task SeedAsync(IServiceProvider serviceProvider)
    {
      using var scope = serviceProvider.CreateScope();
      var context = scope.ServiceProvider.GetRequiredService<YesLocationDbContext>();
      var configuration = serviceProvider.GetRequiredService<IConfiguration>();
      var authService = scope.ServiceProvider.GetRequiredService<IAuthService>();

      string defaultPassword = configuration.GetSection("AppSettings:DefaultPassword").Value!;

      byte[] passwordSalt = authService!.GetPasswordSalt();

      byte[] passwordHash = authService!.GetPasswordHash(defaultPassword, passwordSalt);

      // Appliquer les migrations
      await context.Database.MigrateAsync();

      // Seeding des rôles
      if (!context.Roles.Any())
      {
        var roles = new List<Role>
          {
              new() { Name = "Admin", Description = "Administrator", Value = 7 },
              new() { Name = "User", Description = "Basic user", Value = 3 }
          };
        context.Roles.AddRange(roles);
        await context.SaveChangesAsync();
      }

      // Seeding de l'utilisateur admin
      if (!context.Users.Any(u => u.Username == "admin"))
      {
        var adminUser = new User
        {
          Username = "admin",
          FirstName = "Admin",
          LastName = "User",
          Email = "info@codeattila.ch",
          CreatedAt = DateTime.UtcNow,
          UpdatedAt = DateTime.UtcNow,
          CreatedBy = 1,
          UpdatedBy = 1,
          Auth = new Auth
          {
            PasswordHash = passwordHash,
            PasswordSalt = passwordSalt,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow,
            CreatedBy = 1,
            UpdatedBy = 1
          },
          UserRoles = [
                        new UserRole { RoleId = context.Roles.First(r => r.Name == "Admin").Id },
                        new UserRole { RoleId = context.Roles.First(r => r.Name == "User").Id }
                    ]
        };

        context.Users.Add(adminUser);
        await context.SaveChangesAsync();
      }

      // Seeding de l'agence
      if (!context.Agencies.Any())
      {
        var agency = new Agency();
        context.Agencies.Add(agency);
        await context.SaveChangesAsync();
      }
    }
  }
}
using YesLocation.Domain.Entities;
using YesLocation.Infrastructure.Persistence;

namespace YesLocation.Tests.Common
{
  /// <summary>
  /// Base class for tests that require a database context pre-populated with sample data.
  /// Provides methods to create and seed test data automatically.
  /// </summary>
  public abstract class SeededContextTestBase : ContextTestBase
  {
    protected SeededContextTestBase()
    {
      SeedDatabase(_context);
    }

    /// <summary>
    /// Creates a list of sample users for testing purposes.
    /// </summary>
    /// <param name="count">The number of sample users to create. Defaults to 3.</param>
    /// <returns>A list of User entities with sample data.</returns>
    protected virtual List<User> CreateSampleUsers(int count = 3)
    {
      var users = new List<User>();
      for (int i = 1; i <= count; i++)
      {
        users.Add(new User
        {
          Id = i,
          Username = $"user{i}",
          Email = $"user{i}@example.com",
          FirstName = $"First{i}",
          LastName = $"Last{i}",
        });

        // Add UserRoles to the user 1
        if (i == 1)
        {
          users[0].UserRoles = new List<UserRole>
          {
            new() { RoleId = 1 }, // User
            new() { RoleId = 2 } // Admin
          };

        }

        // Add UserRoles to the user 2
        if (i == 2)
        {
          users[1].UserRoles = new List<UserRole>
          {
            new() { RoleId = 1 } // User
          };

        }

        // Add UserRoles to the user 3
        if (i == 3)
        {
          users[2].UserRoles = new List<UserRole>
          {
            new() { RoleId = 1 }, // User
            new() { RoleId = 3 } // Manager
          };
        }


      }
      return users;

    }

    /// <summary>
    /// Creates a predefined list of sample roles for testing purposes.
    /// </summary>
    /// <returns>A list containing default roles (User, Admin, Manager).</returns>
    protected virtual List<Role> CreateSampleRoles()
    {
      return new List<Role>
      {
        new() { Id = 1, Name = "User", Value = 3 },
        new() { Id = 2, Name = "Admin", Value = 5 },
        new() { Id = 3, Name = "Manager", Value = 7 }
      };
    }

    /// <summary>
    /// Seeds the test database with sample users and roles if they don't already exist.
    /// </summary>
    /// <param name="context">The database context to seed.</param>
    protected virtual void SeedDatabase(YesLocationDbContext context)
    {
      if (!context.Roles.Any())
      {
        var roles = CreateSampleRoles();
        context.Roles.AddRange(roles);
      }

      if (!context.Users.Any())
      {
        var users = CreateSampleUsers();
        context.Users.AddRange(users);
      }
      context.SaveChanges();
    }
  }
}
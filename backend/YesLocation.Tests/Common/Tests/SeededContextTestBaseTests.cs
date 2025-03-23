namespace YesLocation.Tests.Common.Tests;

public class SeededContextTestBaseTests : SeededContextTestBase
{
  [Fact]
  public void CreateSampleUsers_ReturnsCorrectNumberOfUsers()
  {
    // Act
    var users = CreateSampleUsers(5);

    // Assert
    Assert.Equal(5, users.Count);
    Assert.All(users, user => Assert.NotNull(user.Username));
  }

  [Fact]
  public void CreateSampleRoles_ReturnsDefaultRoles()
  {
    // Act
    var roles = CreateSampleRoles();

    // Assert
    Assert.Equal(3, roles.Count);
    Assert.Contains(roles, r => r.Name == "User");
    Assert.Contains(roles, r => r.Name == "Admin");
    Assert.Contains(roles, r => r.Name == "Manager");
  }

  [Fact]
  public void SeedDatabase_PopulatesDatabase()
  {
    // Assert
    Assert.True(_context.Users.Any());
    Assert.True(_context.Roles.Any());
  }
}

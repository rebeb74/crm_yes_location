using Microsoft.EntityFrameworkCore;
using Moq;
using YesLocation.Domain.Entities;
using YesLocation.Domain.Interfaces;
using YesLocation.Infrastructure.Persistence;

namespace YesLocation.Tests.YesLocation.Infrastructure.Tests.Persistence;

public class YesLocationDbContextTests
{
  private readonly DbContextOptions<YesLocationDbContext> _contextOptions;
  private readonly Mock<ICurrentUserService> _mockCurrentUserService;
  public YesLocationDbContextTests()
  {
    _contextOptions = new DbContextOptionsBuilder<YesLocationDbContext>()
        .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
        .Options;

    _mockCurrentUserService = new Mock<ICurrentUserService>();
    _mockCurrentUserService.Setup(c => c.Id).Returns(123);
    _mockCurrentUserService.Setup(c => c.Username).Returns("testuser");
    _mockCurrentUserService.Setup(c => c.Email).Returns("testuser@example.com");
    _mockCurrentUserService.Setup(c => c.FirstName).Returns("test");
    _mockCurrentUserService.Setup(c => c.LastName).Returns("user");
  }

  [Fact]
  public void DbContext_InitializesDbSets()
  {
    // Act
    using var context = new YesLocationDbContext(_contextOptions, _mockCurrentUserService.Object);

    // Assert
    Assert.NotNull(context.Users);
    Assert.NotNull(context.Auth);
  }

  [Fact]
  public async Task SaveChanges_SetsCreatedAtAndUpdatedAt_ForNewEntity()
  {
    // Arrange
    using var context = new YesLocationDbContext(_contextOptions, _mockCurrentUserService.Object);
    var user = new User { Username = "newUserTest", Email = "newUserTest@example.com" };

    // Act
    context.Users.Add(user);
    await context.SaveChangesAsync();

    // Assert
    Assert.NotEqual(DateTime.UtcNow, user.CreatedAt);
    Assert.NotEqual(DateTime.UtcNow, user.UpdatedAt);
    Assert.Equal(123, user.CreatedBy);
    Assert.Equal(123, user.UpdatedBy);
  }

  [Fact]
  public async Task SaveChanges_UpdatesModificationFields_ForModifiedEntity()
  {
    // Arrange
    using var context = new YesLocationDbContext(_contextOptions, _mockCurrentUserService.Object);
    var user = new User { Username = "updatedUserTest", Email = "updatedUserTest@example.com" };
    context.Users.Add(user);
    await context.SaveChangesAsync();

    var initialCreatedAt = user.CreatedAt;
    var initialUpdatedAt = user.UpdatedAt;

    // Act
    user.Username = "Updated userName";
    await Task.Delay(1000); // Attendre un peu pour voir la différence de timestamp
    await context.SaveChangesAsync();
    User? updatedUser = await context.Users.FindAsync(user.Id);

    // Assert
    Assert.NotNull(updatedUser);
    Assert.Equal(initialCreatedAt, updatedUser.CreatedAt);
    Assert.NotEqual(initialUpdatedAt, updatedUser.UpdatedAt);
    Assert.Equal(123, updatedUser.UpdatedBy);
    Assert.NotEqual("updatedUserTest", updatedUser.Username);
  }

  [Fact]
  public void ModelCreating_ConfiguresUserEntity()
  {
    // Act
    using var context = new YesLocationDbContext(_contextOptions, _mockCurrentUserService.Object);
    var userEntity = context.Model.FindEntityType(typeof(User));

    // Assert
    Assert.NotNull(userEntity);
    Assert.Equal("Users", userEntity.GetTableName());
  }
}
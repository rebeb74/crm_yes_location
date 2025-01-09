using Microsoft.EntityFrameworkCore;
using YesLocation.Api.Controllers;
using YesLocation.Infrastructure.Persistence;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Moq;
using YesLocation.Domain.Interfaces;
using YesLocation.Domain.Entities;
using YesLocation.Tests.Common;

namespace YesLocation.Tests.YesLocation.Api.Tests.Controllers;

public class UserControllerTests : SeededContextTestBase
{
  private readonly UserController _controller;

  public UserControllerTests()
  {
    _controller = new UserController(_context);
  }

  [Fact]
  public async Task GetUsers_ReturnsAllUsers()
  {
    // Act
    var result = await _controller.GetUsers();

    // Assert
    var actionResult = Assert.IsType<ActionResult<IEnumerable<User>>>(result);
    var users = Assert.IsType<List<User>>(actionResult.Value);
    Assert.Equal(3, users.Count);
  }

  [Fact]
  public async Task GetUser_WithValidId_ReturnsUser()
  {
    // Act
    var result = await _controller.GetUser(1);

    // Assert
    var actionResult = Assert.IsType<ActionResult<User>>(result);
    var user = Assert.IsType<User>(actionResult.Value);
    Assert.Equal(1, user.Id);
    Assert.Equal("user1", user.Username?.ToLower());
  }

  [Fact]
  public async Task GetUser_WithInvalidId_ReturnsNotFound()
  {
    // Act
    var result = await _controller.GetUser(999);

    // Assert
    Assert.IsType<NotFoundResult>(result.Result);
  }

  [Fact]
  public async Task PostUser_WithValidUser_ReturnsCreatedAtAction()
  {
    // Arrange
    var newUser = new User
    {
      Username = "newuser",
      Email = "newuser@test.com",
      FirstName = "New",
      LastName = "User"
    };

    // Act
    var result = await _controller.PostUser(newUser);

    // Assert
    var actionResult = Assert.IsType<ActionResult<User>>(result);
    var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(actionResult.Result);
    var returnedUser = Assert.IsType<User>(createdAtActionResult.Value);
    Assert.Equal(newUser.Username?.ToLower(), returnedUser.Username?.ToLower());
  }

  [Fact]
  public async Task PutUser_WithValidIdAndUser_ReturnsNoContent()
  {
    // Arrange
    var user = await _context.Users.FindAsync(1);
    user!.FirstName = "Updated";

    // Act
    var result = await _controller.PutUser(1, user);

    // Assert
    var actionResult = Assert.IsType<ActionResult<User>>(result);
    var updatedUser = Assert.IsType<User>(actionResult.Value);
    Assert.Equal("Updated", updatedUser?.FirstName);
  }

  [Fact]
  public async Task DeleteUser_WithValidId_ReturnsNoContent()
  {
    // Act
    var result = await _controller.DeleteUser(1);

    // Assert
    Assert.IsType<NoContentResult>(result);
    Assert.Null(await _context.Users.FindAsync(1));
  }

  [Fact]
  public async Task DeleteUser_WithInvalidId_ReturnsNotFound()
  {
    // Act
    var result = await _controller.DeleteUser(999);

    // Assert
    Assert.IsType<NotFoundResult>(result);
  }
}

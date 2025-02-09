using Microsoft.EntityFrameworkCore;
using YesLocation.Api.Controllers;
using Microsoft.AspNetCore.Mvc;
using YesLocation.Domain.Entities;
using YesLocation.Tests.Common;
using YesLocation.Application.DTOs.User;
using YesLocation.Application.DTOs.Role;

namespace YesLocation.Tests.YesLocation.Api.Tests.Controllers;

public class UserControllerTests : SeededContextTestBase
{
  private readonly UserController _controller;

  public UserControllerTests()
  {
    _controller = new UserController(_context, _mapper);
  }

  [Fact]
  public async Task Update_WithExistingEmail_ReturnsBadRequest()
  {
    // Arrange
    var updateUserDto = new UserCreateDto
    {
      Id = 1,
      Username = "user1",
      Email = "user2@example.com", // Email from another user
      FirstName = "Updated",
      LastName = "User"
    };

    // Act
    var result = await _controller.Update(1, updateUserDto);

    // Assert
    var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
    Assert.Equal("A user with the same email already exists.", badRequestResult.Value);
  }

  [Fact]
  public async Task Update_WithExistingUsername_ReturnsBadRequest()
  {
    // Arrange
    var updateUserDto = new UserCreateDto
    {
      Id = 1,
      Username = "user2", // Username of another user
      Email = "user1@example.com",
      FirstName = "Updated",
      LastName = "User"
    };

    // Act
    var result = await _controller.Update(1, updateUserDto);

    // Assert
    var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
    Assert.Equal("A user with the same username already exists.", badRequestResult.Value);
  }

  [Fact]
  public async Task AddUserRoles_WithValidData_ReturnsOk()
  {
    // Arrange
    var roleIds = new List<int> { 1, 2 };

    // Act
    var result = await _controller.AddUserRoles(1, roleIds);

    // Assert
    Assert.IsType<OkResult>(result);
    var user = await _context.Users.Include(u => u.UserRoles).FirstAsync(u => u.Id == 1);
    Assert.Contains(user.UserRoles, ur => roleIds.Contains(ur.RoleId));
  }

  [Fact]
  public async Task RemoveUserRole_WithValidData_ReturnsNoContent()
  {
    // Act
    var result = await _controller.RemoveUserRole(1, 1);

    // Assert
    Assert.IsType<NoContentResult>(result);
    var userRole = await _context.Set<UserRole>().FindAsync(1, 1);
    Assert.Null(userRole);
  }

  [Fact]
  public async Task GetUserRoles_ReturnsRoles()
  {
    // Act
    var result = await _controller.GetUserRoles(1);

    // Assert
    var actionResult = Assert.IsType<ActionResult<IEnumerable<RoleDto>>>(result);
    var okResult = Assert.IsType<OkObjectResult>(actionResult.Result);
    var roles = Assert.IsAssignableFrom<IEnumerable<RoleDto>>(okResult.Value);
    Assert.NotEmpty(roles);
  }
}

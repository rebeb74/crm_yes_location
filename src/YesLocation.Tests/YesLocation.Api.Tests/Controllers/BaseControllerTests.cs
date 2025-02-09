using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using YesLocation.Domain.Entities;
using YesLocation.Domain.Interfaces;
using YesLocation.Tests.Common;
using YesLocation.Application.DTOs.User;
using YesLocation.Api.Controllers;

namespace YesLocation.Tests.YesLocation.Api.Tests.Controllers;

public class BaseControllerTests : SeededContextTestBase
{
  private readonly UserController _controller;

  public BaseControllerTests()
  {
    _controller = new UserController(_context, _mapper);
  }

  [Fact]
  public async Task GetAll_ReturnsAllEntities()
  {
    // Act
    var result = await _controller.GetAll();

    // Assert
    var actionResult = Assert.IsType<ActionResult<IEnumerable<UserDto>>>(result);
    var okResult = Assert.IsType<OkObjectResult>(actionResult.Result);
    var users = Assert.IsAssignableFrom<IEnumerable<UserDto>>(okResult.Value);
    Assert.NotEmpty(users);
  }

  [Fact]
  public async Task GetById_WithValidId_ReturnsEntity()
  {
    // Act
    var result = await _controller.GetById(1);

    // Assert
    var actionResult = Assert.IsType<ActionResult<UserDto>>(result);
    var okResult = Assert.IsType<OkObjectResult>(actionResult.Result);
    var user = Assert.IsType<UserDto>(okResult.Value);
    Assert.Equal(1, user.Id);
  }

  [Fact]
  public async Task Create_WithValidEntity_ReturnsCreatedAtAction()
  {
    // Arrange
    var newUserDto = new UserCreateDto
    {
      Username = "newuser",
      Email = "newuser@test.com",
      FirstName = "New",
      LastName = "User"
    };

    // Act
    var result = await _controller.Create(newUserDto);

    // Assert
    var actionResult = Assert.IsType<ActionResult<UserDto>>(result);
    var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(actionResult.Result);
    var returnedUser = Assert.IsType<UserDto>(createdAtActionResult.Value);
    Assert.Equal("Newuser", returnedUser.Username);

  }

  [Fact]
  public async Task Update_WithValidIdAndEntity_ReturnsNoContent()
  {
    // Arrange
    var updateUserDto = new UserCreateDto
    {
      Id = 1,
      Username = "updated",
      Email = "updated@test.com",
      FirstName = "Updated",
      LastName = "User"
    };

    // Act
    var result = await _controller.Update(1, updateUserDto);

    // Assert
    Assert.IsType<NoContentResult>(result);
    var updatedEntity = await _context.Users.FindAsync(1);
    Assert.Equal("Updated", updatedEntity?.Username);
  }

  [Fact]
  public async Task Delete_WithValidId_ReturnsNoContent()
  {
    // Act
    var result = await _controller.Delete(1);

    // Assert
    Assert.IsType<NoContentResult>(result);
    var deletedEntity = await _context.Users.FindAsync(1);
    Assert.Null(deletedEntity);
  }

  [Fact]
  public async Task Delete_WithInvalidId_ReturnsNotFound()
  {
    // Act
    var result = await _controller.Delete(999);

    // Assert
    Assert.IsType<NotFoundResult>(result);
  }
}

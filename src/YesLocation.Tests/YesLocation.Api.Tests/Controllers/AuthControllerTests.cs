using Microsoft.EntityFrameworkCore;
using YesLocation.Api.Controllers;
using YesLocation.Application.DTOs;
using YesLocation.Infrastructure.Persistence;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Moq;
using YesLocation.Domain.Interfaces;
using YesLocation.Application.DTOs.User;
using YesLocation.Tests.Common;

namespace YesLocation.Tests.YesLocation.Api.Tests.Controllers;
public class AuthControllerTests : SeededContextTestBase
{
  private readonly AuthController _controller;

  public AuthControllerTests()
  {
    _controller = new AuthController(_context, _configuration);
  }

  [Fact]
  public async Task Register_ValidUser_ReturnsOk()
  {
    // Arrange
    var userDto = new UserRegistrationDto
    {
      Username = "newuser",
      Email = "newuser@example.com",
      Password = "Password123",
      PasswordConfirm = "Password123",
      FirstName = "John",
      LastName = "Doe"
    };

    // Act
    var result = await _controller.Register(userDto);

    // Assert 
    Assert.IsType<OkResult>(result);

    var authCount = await _context.Auth.CountAsync();
    Assert.Equal(1, authCount);

    var auth = await _context.Auth.LastAsync();
    Assert.NotNull(auth);
    Assert.Equal(char.ToUpper(userDto.Username[0]) + userDto.Username[1..], auth.User.Username);
    Assert.Equal(userDto.Email, auth.User.Email);
    Assert.NotNull(auth.PasswordHash);
    Assert.NotNull(auth.PasswordSalt);

    // Vérifie que les contraintes uniques sont bien appliquées
    var duplicateUserDto = new UserRegistrationDto
    {
      Username = userDto.Username,
      Email = "another@example.com",
      Password = "Password123",
      PasswordConfirm = "Password123"
    };
    var duplicateResult = await _controller.Register(duplicateUserDto);
    Assert.IsType<BadRequestObjectResult>(duplicateResult);
  }
}
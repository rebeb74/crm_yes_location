using Microsoft.EntityFrameworkCore;
using YesLocation.Api.Controllers;
using YesLocation.Application.DTOs;
using YesLocation.Infrastructure.Persistence;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Moq;
using YesLocation.Domain.Interfaces;

namespace YesLocation.Tests.YesLocation.Api.Tests.Controllers;
public class AuthControllerTests
{
  private readonly YesLocationDbContext _context;
  private readonly AuthController _controller;
  private readonly DbContextOptions<YesLocationDbContext> _contextOptions;

  public AuthControllerTests()
  {
    // Configuration des mocks
    _contextOptions = new DbContextOptionsBuilder<YesLocationDbContext>()
        .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
        .Options;

    var mockCurrentUserService = new Mock<ICurrentUserService>();

    Mock<IConfiguration> configurationMock = new();
    var configurationSection = new Mock<IConfigurationSection>();
    configurationSection.Setup(x => x.Value).Returns("test-key");
    configurationMock.Setup(x => x.GetSection("AppSettings:TokenKey"))
                     .Returns(configurationSection.Object);
    configurationMock.Setup(x => x.GetSection("AppSettings:PasswordKey"))
                     .Returns(configurationSection.Object);

    _context = new YesLocationDbContext(_contextOptions, mockCurrentUserService.Object);

    // Création du controller avec le DbContext injecté
    _controller = new AuthController(_context, configurationMock.Object);
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

    var auth = await _context.Auth.FirstAsync();
    Assert.NotNull(auth);
    Assert.Equal(char.ToUpper(userDto.Username[0]) + userDto.Username[1..], auth.Username);
    Assert.Equal(userDto.Email, auth.Email);
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
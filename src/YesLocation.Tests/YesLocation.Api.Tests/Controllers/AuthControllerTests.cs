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
  private readonly Mock<IConfiguration> _configurationMock;
  private readonly Mock<IEnvironmentService> _environmentServiceMock;

  public AuthControllerTests()
  {
    var options = new DbContextOptionsBuilder<YesLocationDbContext>()
        .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
        .Options;
    var mockCurrentUserService = new Mock<ICurrentUserService>();
    _context = new YesLocationDbContext(options, mockCurrentUserService.Object);

    // Configuration des mocks
    _configurationMock = new Mock<IConfiguration>();
    _environmentServiceMock = new Mock<IEnvironmentService>();

    var configurationSection = new Mock<IConfigurationSection>();
    configurationSection.Setup(x => x.Value).Returns("test-key");
    _configurationMock.Setup(x => x.GetSection("AppSettings:TokenKey"))
                     .Returns(configurationSection.Object);
    _configurationMock.Setup(x => x.GetSection("AppSettings:PasswordKey"))
                     .Returns(configurationSection.Object);

    // Création du controller avec le DbContext injecté
    _controller = new AuthController(_context, _configurationMock.Object, _environmentServiceMock.Object);
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
using System.Security.Cryptography;
using Microsoft.Extensions.Configuration;
using Moq;
using YesLocation.Api.Services;
using YesLocation.Domain.Entities;
using YesLocation.Domain.Interfaces;

namespace YesLocation.Tests.YesLocation.Api.Tests.Services;
public class AuthServiceTests
{
  private readonly IAuthService _authService;

  private readonly Mock<IConfiguration> _configurationMock;

  public AuthServiceTests()
  {
    _configurationMock = new Mock<IConfiguration>();
    var configurationSection = new Mock<IConfigurationSection>();
    configurationSection.Setup(x => x.Value).Returns("une_cle_secrete_suffisamment_longue_pour_le_test_12345_une_cle_secrete_suffisamment_longue_pour_le_test_12345_une_cle_secrete_suffisamment_longue_pour_le_test_12345");

    _configurationMock.Setup(x => x.GetSection("AppSettings:TokenKey"))
                     .Returns(configurationSection.Object);
    _configurationMock.Setup(x => x.GetSection("AppSettings:PasswordKey"))
                     .Returns(configurationSection.Object);

    _configurationMock.Setup(x => x["Jwt:Key"]).Returns("votre_clé_secrète_de_test_suffisamment_longue");
    _configurationMock.Setup(x => x["Jwt:Issuer"]).Returns("test_issuer");
    _configurationMock.Setup(x => x["Jwt:Audience"]).Returns("test_audience");

    _authService = new AuthService(_configurationMock.Object);
  }

  [Fact]
  public void GetPasswordHash_ReturnsValidHash()
  {
    // Arrange
    string password = "TestPassword123";
    byte[] passwordSalt = new byte[16];
    using (RandomNumberGenerator rng = RandomNumberGenerator.Create())
    {
      rng.GetNonZeroBytes(passwordSalt);
    }

    // Act
    byte[] hashedPassword = _authService.GetPasswordHash(password, passwordSalt);

    // Assert
    Assert.NotNull(hashedPassword);
    Assert.NotEmpty(hashedPassword);
  }

  [Fact]
  public void VerifyPassword_WithCorrectPassword_ReturnsTrue()
  {
    // Arrange
    string password = "TestPassword123";
    byte[] passwordSalt = new byte[16];
    using (RandomNumberGenerator rng = RandomNumberGenerator.Create())
    {
      rng.GetNonZeroBytes(passwordSalt);
    }
    byte[] hashedPassword = _authService.GetPasswordHash(password, passwordSalt);

    // Act
    bool result = _authService.VerifyPassword(hashedPassword, hashedPassword);

    // Assert
    Assert.True(result);
  }

  [Fact]
  public void VerifyPassword_WithIncorrectPassword_ReturnsFalse()
  {
    // Arrange
    string correctPassword = "TestPassword123";
    string wrongPassword = "WrongPassword123";
    byte[] passwordSalt = new byte[16];
    using (RandomNumberGenerator rng = RandomNumberGenerator.Create())
    {
      rng.GetNonZeroBytes(passwordSalt);
    }
    byte[] hashedCorrectPassword = _authService.GetPasswordHash(correctPassword, passwordSalt);
    byte[] hashedWrongPassword = _authService.GetPasswordHash(wrongPassword, passwordSalt);

    // Act
    bool result = _authService.VerifyPassword(hashedCorrectPassword, hashedWrongPassword);

    // Assert
    Assert.False(result);
  }

  [Fact]
  public void CreateToken_ReturnsValidToken()
  {
    // Arrange
    User user = new()
    {
      Id = 1,
      Username = "testUser",
      Email = "testuser@example.com",
      FirstName = "Test",
      LastName = "User"
    };

    // Act
    string token = _authService.CreateToken(user);

    // Assert
    Assert.NotNull(token);
    Assert.NotEmpty(token);
  }

  [Theory]
  [InlineData("user@example.com", true)]
  [InlineData("invalid-email", false)]
  [InlineData("", false)]
  [InlineData(null, false)]
  public void ValidateEmail_ReturnsExpectedResult(string email, bool expectedResult)
  {
    // Act
    bool isValid = _authService.ValidateEmail(email);

    // Assert
    Assert.Equal(expectedResult, isValid);
  }

}
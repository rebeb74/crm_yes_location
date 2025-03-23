using System.Security.Cryptography;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Moq;
using YesLocation.Api.Services;
using YesLocation.Domain.Entities;
using YesLocation.Domain.Interfaces;
using YesLocation.Infrastructure.Persistence;
using YesLocation.Tests.Common;

namespace YesLocation.Tests.YesLocation.Api.Tests.Services;
public class AuthServiceTests : SeededContextTestBase
{
  private readonly AuthService _authService;

  public AuthServiceTests()
  {
    _authService = new AuthService(_configuration);
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
      Id = 999,
      Username = "testUser",
      Email = "testuser@example.com",
      FirstName = "Test",
      LastName = "User",
      UserRoles = new List<UserRole>
            {
                new UserRole
                {
                    UserId = 999,
                    RoleId = _context.Roles.First(r => r.Name == "User").Id,
                    Role = _context.Roles.First(r => r.Name == "User")
                }
            }
    };

    // Act
    string token = _authService.CreateToken(user);

    // Assert
    Assert.NotNull(token);
    Assert.NotEmpty(token);

    // Optionnel : Vérifier que le token peut être lu et contient les informations attendues
    var tokenHandler = new System.IdentityModel.Tokens.Jwt.JwtSecurityTokenHandler();
    var jwtToken = tokenHandler.ReadJwtToken(token);

    Assert.Equal("https://test-issuer.com", jwtToken.Issuer);
    Assert.Contains("https://test-audience.com", jwtToken.Audiences);

    // Vérifier que l'utilisateur a bien des rôles assignés
    Assert.NotEmpty(user.UserRoles);
    Assert.All(user.UserRoles, ur => Assert.NotNull(ur.Role));

    // Vérifier la présence du claim "role"
    var roleClaims = jwtToken.Claims.Where(c => c.Type == "role").ToList();
    Assert.NotEmpty(roleClaims);
    Assert.Contains(roleClaims, c => c.Value == "User");
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
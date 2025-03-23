using Microsoft.EntityFrameworkCore;

namespace YesLocation.Tests.Common.Tests;

public class ContextTestBaseTests : ContextTestBase
{
  [Fact]
  public void CreateTestConfiguration_ContainsRequiredSettings()
  {
    // Assert
    Assert.NotNull(_configuration["Jwt:Issuer"]);
    Assert.NotNull(_configuration["Jwt:Audience"]);
    Assert.NotNull(_configuration["Jwt:TokenKey"]);
    Assert.NotNull(_configuration["Jwt:PasswordKey"]);
  }

  [Fact]
  public void CreateMockCurrentUserService_ReturnsValidMock()
  {
    // Assert
    Assert.Equal(123, _currentUserService.Object.Id);
    Assert.Equal("testuser", _currentUserService.Object.Username);
    Assert.Equal("testuser@example.com", _currentUserService.Object.Email);
  }

  [Fact]
  public void CreateDbContext_ReturnsWorkingContext()
  {
    // Assert
    Assert.NotNull(_context);
    Assert.True(_context.Database.IsInMemory());
  }
}

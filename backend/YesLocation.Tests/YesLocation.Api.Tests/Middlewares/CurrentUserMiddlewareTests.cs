using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Moq;
using YesLocation.Api.Middlewares;
using YesLocation.Domain.Interfaces;

namespace YesLocation.Tests.YesLocation.Api.Tests.Middlewares;
public class CurrentUserMiddlewareTests
{
  [Fact]
  public async Task Invoke_SetsCurrentUserProperties_WhenUserIsAuthenticated()
  {
    // Arrange
    var mockCurrentUserService = new Mock<ICurrentUserService>();
    Claim[] claims =
      [
        new("userId", "123"),
        new("username", "testuser"),
        new("email", "testuser@example.com"),
        new("firstName", "Test"),
        new("lastName", "User")
      ];
    var identity = new ClaimsIdentity(claims, "TestAuthType");
    var claimsPrincipal = new ClaimsPrincipal(identity);

    var context = new DefaultHttpContext
    {
      User = claimsPrincipal
    };

    var mockRequestDelegate = new Mock<RequestDelegate>();
    var middleware = new CurrentUserMiddleware(mockRequestDelegate.Object);

    // Act
    await middleware.Invoke(context, mockCurrentUserService.Object);

    // Assert
    mockCurrentUserService.VerifySet(c => c.Id = 123, Times.Once);
    mockCurrentUserService.VerifySet(c => c.Username = "testuser", Times.Once);
    mockCurrentUserService.VerifySet(c => c.Email = "testuser@example.com", Times.Once);
    mockCurrentUserService.VerifySet(c => c.FirstName = "Test", Times.Once);
    mockCurrentUserService.VerifySet(c => c.LastName = "User", Times.Once);
  }

  [Fact]
  public async Task Invoke_DoesNotSetCurrentUserProperties_WhenUserIsNotAuthenticated()
  {
    // Arrange
    var mockCurrentUserService = new Mock<ICurrentUserService>();
    var context = new DefaultHttpContext();

    var mockRequestDelegate = new Mock<RequestDelegate>();
    var middleware = new CurrentUserMiddleware(mockRequestDelegate.Object);

    // Act
    await middleware.Invoke(context, mockCurrentUserService.Object);

    // Assert
    mockCurrentUserService.VerifySet(c => c.Id = It.IsAny<int?>(), Times.Never);
    mockCurrentUserService.VerifySet(c => c.Username = It.IsAny<string>(), Times.Never);
    mockCurrentUserService.VerifySet(c => c.Email = It.IsAny<string>(), Times.Never);
    mockCurrentUserService.VerifySet(c => c.FirstName = It.IsAny<string>(), Times.Never);
    mockCurrentUserService.VerifySet(c => c.LastName = It.IsAny<string>(), Times.Never);
  }
}
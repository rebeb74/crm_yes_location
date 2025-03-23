using System;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Moq;
using YesLocation.Api.Mappings;
using YesLocation.Domain.Interfaces;
using YesLocation.Infrastructure.Persistence;

namespace YesLocation.Tests.Common;

/// <summary>
/// Base class for test contexts that provides a configured database context, configuration settings,
/// and mock user service. Implements IDisposable to ensure proper cleanup of database resources.
/// </summary>
public abstract class ContextTestBase : IDisposable
{
  protected readonly YesLocationDbContext _context;
  protected readonly IConfiguration _configuration;
  protected readonly Mock<ICurrentUserService> _currentUserService;
  protected readonly IMapper _mapper;

  protected ContextTestBase()
  {
    _configuration = CreateTestConfiguration();
    _currentUserService = CreateMockCurrentUserService();
    _context = CreateDbContext(Guid.NewGuid().ToString());
    // Configuration du vrai AutoMapper
    var config = new MapperConfiguration(cfg =>
    {
      cfg.AddProfile<MappingProfile>();
    });

    _mapper = config.CreateMapper();
  }


  /// <summary>
  /// Creates a test configuration with predefined JWT settings for testing purposes.
  /// </summary>
  /// <returns>An IConfiguration instance with in-memory test settings.</returns>
  protected virtual IConfiguration CreateTestConfiguration()
  {
    var inMemorySettings = new Dictionary<string, string?> {
            {"Jwt:Issuer", "https://test-issuer.com"},
            {"Jwt:Audience", "https://test-audience.com"},
            {"Jwt:TokenKey", "Test_Secret_Key_1234567890_Test_Secret_Key_1234567890_Test_Secret_Key_1234567890_Test_Secret_Key_1234567890_Test_Secret_Key_1234567890_Test_Secret_Key_1234567890_Test_Secret_Key_1234567890_Test_Secret_Key_1234567890"},
            {"Jwt:PasswordKey", "Test_Password_Key_1234567890"}
        };

    return new ConfigurationBuilder()
        .AddInMemoryCollection(inMemorySettings)
        .Build();
  }

  /// <summary>
  /// Creates a mock implementation of ICurrentUserService for testing.
  /// </summary>
  /// <param name="userId">The user ID to mock. Defaults to 123.</param>
  /// <param name="username">The username to mock. Defaults to "testuser".</param>
  /// <param name="email">The email to mock. Defaults to "testuser@example.com".</param>
  /// <param name="firstName">The first name to mock. Defaults to "test".</param>
  /// <param name="lastName">The last name to mock. Defaults to "user".</param>
  /// <returns>A configured mock of ICurrentUserService.</returns>
  protected virtual Mock<ICurrentUserService> CreateMockCurrentUserService(
      int userId = 123,
      string username = "testuser",
      string email = "testuser@example.com",
      string firstName = "test",
      string lastName = "user")
  {
    var mock = new Mock<ICurrentUserService>();
    mock.Setup(c => c.Id).Returns(userId);
    mock.Setup(c => c.Username).Returns(username);
    mock.Setup(c => c.Email).Returns(email);
    mock.Setup(c => c.FirstName).Returns(firstName);
    mock.Setup(c => c.LastName).Returns(lastName);
    return mock;
  }

  /// <summary>
  /// Creates a new in-memory database context for testing.
  /// </summary>
  /// <param name="dbName">The unique name for the in-memory database.</param>
  /// <returns>A configured YesLocationDbContext instance.</returns>
  protected virtual YesLocationDbContext CreateDbContext(string dbName)
  {
    var options = new DbContextOptionsBuilder<YesLocationDbContext>()
        .UseInMemoryDatabase(databaseName: dbName)
        .Options;

    return new YesLocationDbContext(options, _currentUserService.Object);
  }

  /// <summary>
  /// Cleans up the test database and disposes of the database context.
  /// </summary>
  public void Dispose()
  {
    _context.Database.EnsureDeleted();
    _context.Dispose();
  }
}
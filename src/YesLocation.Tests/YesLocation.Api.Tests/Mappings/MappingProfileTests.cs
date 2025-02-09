using AutoMapper;
using YesLocation.Api.Mappings;
using YesLocation.Domain.Entities;
using YesLocation.Application.DTOs.User;
using YesLocation.Application.DTOs.Role;

namespace YesLocation.Tests.YesLocation.Api.Tests.Mappings;

public class MappingProfileTests
{
  private readonly IMapper _mapper;

  public MappingProfileTests()
  {
    var config = new MapperConfiguration(cfg => cfg.AddProfile<MappingProfile>());
    _mapper = config.CreateMapper();
  }

  [Fact]
  public void AutoMapper_Configuration_IsValid()
  {
    var config = new MapperConfiguration(cfg => cfg.AddProfile<MappingProfile>());
    config.AssertConfigurationIsValid();
  }

  [Fact]
  public void Map_User_To_UserDto_HandlesSpecialCases()
  {
    // Arrange
    var user = new User
    {
      Username = "testuser",
      Email = "TEST@example.com",
    };

    // Act
    var userDto = _mapper.Map<UserDto>(user);

    // Assert
    Assert.Equal("Testuser", userDto.Username); // Test capitalisation
    Assert.Equal("test@example.com", userDto.Email); // Test lowercase
  }

  [Fact]
  public void Map_SimpleEntity_Works()
  {
    // Arrange
    var role = new Role { Id = 1, Name = "Admin" };

    // Act
    var roleDto = _mapper.Map<RoleDto>(role);

    // Assert
    Assert.Equal(role.Id, roleDto.Id);
    Assert.Equal(role.Name, roleDto.Name);
  }

  [Fact]
  public void Map_User_To_UserDto_IsValid()
  {
    // Arrange
    var user = new User
    {
      Id = 1,
      Username = "testuser",
      Email = "test@example.com",
      FirstName = "Test",
      LastName = "User",
      CreatedAt = DateTime.UtcNow,
      UpdatedAt = DateTime.UtcNow
    };

    // Act
    var userDto = _mapper.Map<UserDto>(user);

    // Assert
    Assert.Equal(user.Id, userDto.Id);
    Assert.Equal("Testuser", userDto.Username); // Note the logical capitalization in User.cs
    Assert.Equal("test@example.com", userDto.Email.ToLower());
    Assert.Equal(user.FirstName, userDto.FirstName);
    Assert.Equal(user.LastName, userDto.LastName);
    Assert.Equal(user.CreatedAt, userDto.CreatedAt);
    Assert.Equal(user.UpdatedAt, userDto.UpdatedAt);
  }

  [Fact]
  public void Map_UserCreateDto_To_User_IsValid()
  {
    // Arrange
    var userCreateDto = new UserCreateDto
    {
      Username = "newuser",
      Email = "new@example.com",
      FirstName = "New",
      LastName = "User"
    };

    // Act
    var user = _mapper.Map<User>(userCreateDto);

    // Assert
    Assert.Equal("Newuser", user.Username); // Vérifie la capitalisation
    Assert.Equal("new@example.com", user.Email?.ToLower());
    Assert.Equal(userCreateDto.FirstName, user.FirstName);
    Assert.Equal(userCreateDto.LastName, user.LastName);
  }

  [Fact]
  public void Map_User_To_UserCreateDto_IsValid()
  {
    // Arrange
    var user = new User
    {
      Id = 1,
      Username = "testuser",
      Email = "test@example.com",
      FirstName = "Test",
      LastName = "User"
    };

    // Act
    var userCreateDto = _mapper.Map<UserCreateDto>(user);

    // Assert
    Assert.Equal(user.Id, userCreateDto.Id);
    Assert.Equal("Testuser", userCreateDto.Username);
    Assert.Equal("test@example.com", userCreateDto.Email.ToLower());
    Assert.Equal(user.FirstName, userCreateDto.FirstName);
    Assert.Equal(user.LastName, userCreateDto.LastName);
  }

  [Fact]
  public void Map_Role_Mappings_AreValid()
  {
    // Arrange
    var role = new Role
    {
      Id = 1,
      Name = "Admin"
    };

    // Act
    var roleDto = _mapper.Map<RoleDto>(role);

    // Assert
    Assert.Equal(role.Id, roleDto.Id);
    Assert.Equal(role.Name, roleDto.Name);
  }
}
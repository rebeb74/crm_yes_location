using System.ComponentModel.DataAnnotations;
using Xunit.Sdk;
using YesLocation.Application.DTOs;
using YesLocation.Application.DTOs.User;

namespace YesLocation.Tests.YesLocation.Application.Tests.DTOs;
public class UserRegistrationDtoTests
{
  [Theory]
  [InlineData("testuser", "test@example.com", "Test1234", "Test1234", "Jean", "Dupont", "Username", true)]
  [InlineData("", "test@example.com", "Test1234", "Test1234", "Jean", "Dupont", "Username", false)]
  [InlineData("testuser", "", "Test1234", "Test1234", "Jean", "Dupont", "Email", false)]
  [InlineData("testuser", "test@example.com", "", "Test1234", "Jean", "Dupont", "Password", false)]
  [InlineData("testuser", "test@example.com", "Test1234", "", "Jean", "Dupont", "PasswordConfirm", false)]
  public void UserRegistrationDto_MissingRequiredField_ShouldBeInvalid(
    string username,
    string email,
    string password,
    string passwordConfirm,
    string firstName,
    string lastName,
    string expectedFailingMember,
    bool expectedIsValid
    )
  {
    // Arrange
    var dto = new UserRegistrationDto
    {
      Username = username,
      Email = email,
      Password = password,
      PasswordConfirm = passwordConfirm,
      FirstName = firstName,
      LastName = lastName
    };

    // Act
    var results = ValidateModel(dto);

    // Assert
    if (expectedIsValid)
      Assert.Empty(results);
    else
    {
      Assert.NotEmpty(results);
      Assert.Equal(expectedFailingMember, results[0].MemberNames.First());
    }
  }

  private static IList<ValidationResult> ValidateModel(object model)
  {
    var results = new List<ValidationResult>();
    var context = new ValidationContext(model, serviceProvider: null, items: null);
    Validator.TryValidateObject(model, context, results, true);
    return results;
  }
}
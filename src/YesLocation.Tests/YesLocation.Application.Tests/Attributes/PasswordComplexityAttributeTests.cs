using System.ComponentModel.DataAnnotations;
using YesLocation.Application.Attributes;

namespace YesLocation.Tests.YesLocation.Application.Tests.Attributes;

public class PasswordComplexityAttributeTests
{


  public PasswordComplexityAttributeTests()
  {

  }

  [Theory]
  [InlineData("testPassword123", true)]  // Cast explicite en object
  [InlineData("TestPassword", false)]
  [InlineData("testpassword123", false)]
  [InlineData("TESTPASSWORD123", false)]
  public void IsValid_ReturnsExpectedResult(object? value, bool expected)
  {
    // Arrange
    var passwordComplexityAttribute = new PasswordComplexityAttribute();
    var validationContext = new ValidationContext(new object());

    // Act
    try
    {
      passwordComplexityAttribute.Validate(value, validationContext);
      // Assert
      Assert.True(expected);
    }
    catch (ValidationException)
    {
      // Assert
      Assert.False(expected);
    }
  }

  [Theory]
  [InlineData("password123", false)]      // string
  [InlineData(123, false)]                // int
  [InlineData(new byte[] { 1, 2, 3 }, false)] // byte[]
  public void IsValid_WithDifferentTypes_ReturnsExpectedResult(object? value, bool expected)
  {
    // Arrange
    var passwordComplexityAttribute = new PasswordComplexityAttribute();
    var validationContext = new ValidationContext(new object());

    // Act
    try
    {
      passwordComplexityAttribute.Validate(value, validationContext);
      // Assert
      Assert.True(expected);
    }
    catch (ValidationException)
    {
      // Assert
      Assert.False(expected);
    }
  }
}
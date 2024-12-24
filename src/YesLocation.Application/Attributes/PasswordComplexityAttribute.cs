using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace YesLocation.Application.Attributes;

public class PasswordComplexityAttribute : ValidationAttribute
{
  protected override ValidationResult IsValid(object? value, ValidationContext validationContext)
  {
    if (value == null)
    {
      return new ValidationResult("The password is required.");
    }

    var password = value.ToString()!;

    if (!Regex.IsMatch(password, @"[A-Z]+") ||
        !Regex.IsMatch(password, @"[a-z]+") ||
        !Regex.IsMatch(password, @"[0-9]+"))
    {
      return new ValidationResult("The password must contain at least one upper case letter, one lower case letter and one number.");
    }

    return ValidationResult.Success!;
  }
}
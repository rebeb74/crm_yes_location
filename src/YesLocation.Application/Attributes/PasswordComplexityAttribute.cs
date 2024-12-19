using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace YesLocation.Application.Attributes;

public class PasswordComplexityAttribute : ValidationAttribute
{
  protected override ValidationResult IsValid(object? value, ValidationContext validationContext)
  {
    if (value == null)
    {
      return new ValidationResult("Le mot de passe est requis.");
    }

    var password = value.ToString()!;

    if (!Regex.IsMatch(password, @"[A-Z]+") ||
        !Regex.IsMatch(password, @"[a-z]+") ||
        !Regex.IsMatch(password, @"[0-9]+"))
    {
      return new ValidationResult("Le mot de passe doit contenir au moins une lettre majuscule, une lettre minuscule et un chiffre.");
    }

    return ValidationResult.Success!;
  }
}
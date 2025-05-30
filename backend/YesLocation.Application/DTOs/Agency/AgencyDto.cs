using System;
using YesLocation.Application.DTOs.Common;
using YesLocation.Domain.Interfaces;

namespace YesLocation.Application.DTOs.Agency;

public class AgencyDto : BaseModelDto
{
  public string? CompanyName { get; set; }
  public string? SiretNumber { get; set; }
  public string? VatNumber { get; set; }
  public string? PhoneNumber { get; set; }
  public string? Email { get; set; }
  public string? Website { get; set; }

  // Address
  public string? StreetAddress { get; set; }
  public string? PostalCode { get; set; }
  public string? City { get; set; }

  // Other information
  public string? LogoUrl { get; set; }
  public string? BankAccountIban { get; set; }
  public string? BankAccountBic { get; set; }

  // Propriétés de thème (format OKLCH pour compatibilité Tailwind v4)
  public string? PrimaryColor { get; set; }
  public string? SecondaryColor { get; set; }
  public string? AccentColor { get; set; }
  public string? SuccessColor { get; set; }
  public string? WarningColor { get; set; }
  public string? ErrorColor { get; set; }

  // Mode sombre
  public bool UseDarkMode { get; set; }
  public string? DarkModePrimaryColor { get; set; }
  public string? DarkModeSecondaryColor { get; set; }
  public string? DarkModeAccentColor { get; set; }
  public string? DarkModeSuccessColor { get; set; }
  public string? DarkModeWarningColor { get; set; }
  public string? DarkModeErrorColor { get; set; }
}
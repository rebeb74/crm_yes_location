using System.ComponentModel.DataAnnotations;
using YesLocation.Application.DTOs.Common;

namespace YesLocation.Application.DTOs.Agency;

public class AgencyInputDto : BaseInputModelDto
{
  public string CompanyName { get; set; } = string.Empty;
  public string SiretNumber { get; set; } = string.Empty;
  public string? VatNumber { get; set; }
  [Phone]
  public string PhoneNumber { get; set; } = string.Empty;
  [EmailAddress]
  public string Email { get; set; } = string.Empty;
  [Url]
  public string? Website { get; set; }

  // Adresse
  public string StreetAddress { get; set; } = string.Empty;
  public string PostalCode { get; set; } = string.Empty;
  public string City { get; set; } = string.Empty;

  // Autres informations
  [Url]
  public string? LogoUrl { get; set; }
  public string? BankAccountIban { get; set; }
  public string? BankAccountBic { get; set; }
}
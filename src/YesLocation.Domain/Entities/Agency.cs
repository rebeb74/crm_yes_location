namespace YesLocation.Domain.Entities;

public class Agency : BaseModel
{
  public string? CompanyName { get; set; }
  public string? SiretNumber { get; set; }
  public string? VatNumber { get; set; }
  public string? PhoneNumber { get; set; }
  public string? Email { get; set; }
  public string? Website { get; set; }

  // Adresse
  public string? StreetAddress { get; set; }
  public string? PostalCode { get; set; }
  public string? City { get; set; }

  // Autres informations
  public string? LogoUrl { get; set; }
  public string? BankAccountIban { get; set; }
  public string? BankAccountBic { get; set; }
}
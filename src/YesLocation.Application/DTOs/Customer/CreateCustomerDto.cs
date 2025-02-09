using YesLocation.Domain.Enums;
using YesLocation.Domain.Interfaces;

namespace YesLocation.Application.DTOs.Customer;

public class CreateCustomerDto : IBaseDto
{
  public int? Id { get; set; }
  public string? FirstName { get; set; }
  public string? LastName { get; set; }
  public string? Email { get; set; }
  public string? Phone { get; set; }
  public string? Address { get; set; }
  public string? City { get; set; }
  public string? State { get; set; }
  public string? Zip { get; set; }
  public string? Country { get; set; }
  public string? Notes { get; set; }
  public CustomerType? Type { get; set; }
  public string? CompanyName { get; set; }
  public string? VatNumber { get; set; }
  public string? DriverLicenseNumber { get; set; }
  public DateTime? DriverLicenseExpiry { get; set; }
  public string? EmergencyContactName { get; set; }
  public string? EmergencyContactPhone { get; set; }
}

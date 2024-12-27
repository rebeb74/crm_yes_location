using System.ComponentModel.DataAnnotations;
using YesLocation.Application.Attributes;

namespace YesLocation.Application.DTOs
{
  public partial class UserRegistrationDto
  {
    private string? _username;
    [Required(AllowEmptyStrings = false, ErrorMessage = "The user name is required.")]
    [StringLength(50, MinimumLength = 3)]
    public string? Username
    {
      get { return _username; }
      set
      {
        _username = value?.ToLower();
      }
    }

    private string? _Email;
    [Required(AllowEmptyStrings = false, ErrorMessage = "An email address is required.")]
    [EmailAddress(ErrorMessage = "The email address is invalid.")]
    [StringLength(50, MinimumLength = 6)]
    public string? Email
    {
      get { return _Email; }
      set
      {
        _Email = value?.ToLower();
      }
    }

    [Required(AllowEmptyStrings = false, ErrorMessage = "The password is required.")]
    [PasswordComplexity]
    public string Password { get; set; } = "";

    [Required(AllowEmptyStrings = false, ErrorMessage = "The confirmation password is required.")]
    [Compare("Password", ErrorMessage = "The passwords do not match.")]
    public string PasswordConfirm { get; set; } = "";

    private string _FirstName = "";
    [Required(AllowEmptyStrings = false, ErrorMessage = "The first name is required.")]
    [StringLength(50, MinimumLength = 3)]
    public string FirstName
    {
      get { return _FirstName; }
      set
      {
        _FirstName = value.ToLower();
      }
    }

    private string _LastName = "";
    [Required(AllowEmptyStrings = false, ErrorMessage = "The lastname is required.")]
    [StringLength(50, MinimumLength = 3)]
    public string LastName
    {
      get { return _LastName; }
      set
      {
        _LastName = value.ToLower();
      }
    }

  }
}
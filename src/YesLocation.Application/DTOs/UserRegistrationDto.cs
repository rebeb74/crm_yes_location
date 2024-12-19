using System.ComponentModel.DataAnnotations;
using YesLocation.Application.Attributes;

namespace YesLocation.Application.DTOs
{
  public partial class UserRegistrationDto
  {
    private string? _username;
    [Required(AllowEmptyStrings = false, ErrorMessage = "Le nom d'utilisateur est requis.")]
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
    [Required(AllowEmptyStrings = false, ErrorMessage = "L'adresse email est requise.")]
    [EmailAddress(ErrorMessage = "L'adresse email n'est pas valide.")]
    [StringLength(50, MinimumLength = 6)]
    public string? Email
    {
      get { return _Email; }
      set
      {
        _Email = value?.ToLower();
      }
    }

    [Required(AllowEmptyStrings = false, ErrorMessage = "Le mot de passe est requis.")]
    [PasswordComplexity]
    public string Password { get; set; } = "";

    [Required(AllowEmptyStrings = false, ErrorMessage = "Le mot de passe de confirmation est requis.")]
    [Compare("Password", ErrorMessage = "Les mots de passe ne correspondent pas.")]
    public string PasswordConfirm { get; set; } = "";

    private string _FirstName = "";
    [Required(AllowEmptyStrings = false, ErrorMessage = "Le prénom est requis.")]
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
    [Required(AllowEmptyStrings = false, ErrorMessage = "Le nom de famille est requis.")]
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
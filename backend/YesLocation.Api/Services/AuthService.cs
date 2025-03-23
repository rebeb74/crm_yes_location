using System.IdentityModel.Tokens.Jwt;
using System.Net.Mail;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.IdentityModel.Tokens;
using YesLocation.Domain.Entities;
using YesLocation.Domain.Interfaces;

namespace YesLocation.Api.Services;

/// <summary>
/// Service handling authentication-related operations including password hashing, verification, and JWT token generation
/// </summary>
public class AuthService(IConfiguration configuration) : IAuthService
{
  private readonly IConfiguration _configuration = configuration;

  /// <summary>
  /// Generates a password hash using PBKDF2 algorithm
  /// </summary>
  /// <param name="password">The plain text password to hash</param>
  /// <param name="passwordSalt">The salt to use in the hashing process</param>
  /// <returns>The hashed password as a byte array</returns>
  public byte[] GetPasswordHash(string password, byte[] passwordSalt)
  {
    string passwordSaltPlusString = _configuration.GetSection("Jwt:PasswordKey").Value +
        Convert.ToBase64String(passwordSalt);

    return KeyDerivation.Pbkdf2(
        password: password,
        salt: Encoding.ASCII.GetBytes(passwordSaltPlusString),
        prf: KeyDerivationPrf.HMACSHA256,
        iterationCount: 100000,
        numBytesRequested: 256 / 8
    );
  }

  /// <summary>
  /// Verifies if a password hash matches the stored hash
  /// </summary>
  /// <param name="passwordHashDb">The stored password hash from the database</param>
  /// <param name="passwordHash">The password hash to verify</param>
  /// <returns>True if the hashes match, false otherwise</returns>
  public bool VerifyPassword(byte[] passwordHashDb, byte[] passwordHash)
  {
    for (int i = 0; i < passwordHash.Length; i++)
    {
      if (passwordHash[i] != passwordHashDb[i]) return false;
    }

    return true;
  }

  /// <summary>
  /// Creates a JWT token for a user with their claims
  /// </summary>
  /// <param name="user">The user for whom to create the token</param>
  /// <returns>A JWT token string</returns>
  /// <exception cref="ArgumentException">Thrown when JWT TokenKey is missing in configuration</exception>
  public string CreateToken(User user)
  {
    List<Claim> claims = [
            new Claim("userId", user.Id.ToString()),
            new Claim("username", user.Username ?? ""),
            new Claim("email", user.Email ?? ""),
            new Claim("firstName", user.FirstName ?? ""),
            new Claim("lastName", user.LastName ?? "")
        ];

    foreach (var userRole in user.UserRoles)
    {
      claims.Add(new Claim(ClaimTypes.Role, userRole.Role.Name));
    }

    string tokenKeyString = _configuration["Jwt:TokenKey"] ?? "";
    if (string.IsNullOrEmpty(tokenKeyString))
    {
      throw new ArgumentException("JWT TokenKey est manquant dans la configuration.");
    }
    SymmetricSecurityKey key = new(Encoding.UTF8.GetBytes(tokenKeyString));

    SigningCredentials creds = new(key, SecurityAlgorithms.HmacSha512Signature);

    SecurityTokenDescriptor tokenDescriptor = new()
    {
      Subject = new ClaimsIdentity(claims),
      Expires = DateTime.Now.AddDays(1),
      SigningCredentials = creds,
      Issuer = _configuration["Jwt:Issuer"],
      Audience = _configuration["Jwt:Audience"]
    };

    JwtSecurityTokenHandler tokenHandler = new();

    SecurityToken token = tokenHandler.CreateToken(tokenDescriptor);

    return tokenHandler.WriteToken(token);
  }

  /// <summary>
  /// Validates an email address format
  /// </summary>
  /// <param name="email">The email address to validate</param>
  /// <returns>True if the email format is valid, false otherwise</returns>
  public bool ValidateEmail(string email)
  {
    if (string.IsNullOrWhiteSpace(email)) return false;
    try
    {
      MailAddress m = new(email);
      return true;
    }
    catch (FormatException)
    {
      return false;
    }
  }

  /// <summary>
  /// Generates a random password salt
  /// </summary>
  /// <returns>A byte array containing the generated salt</returns>
  public byte[] GetPasswordSalt()
  {
    byte[] passwordSalt = [128 / 8];
    using (RandomNumberGenerator rng = RandomNumberGenerator.Create())
    {
      rng.GetNonZeroBytes(passwordSalt);
    }

    return passwordSalt;
  }
}
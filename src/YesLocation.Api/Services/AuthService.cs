using System.IdentityModel.Tokens.Jwt;
using System.Net.Mail;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.IdentityModel.Tokens;
using YesLocation.Domain.Entities;
using YesLocation.Domain.Interfaces;

namespace YesLocation.Api.Services;
public class AuthService(IConfiguration configuration) : IAuthService
{
  private readonly IConfiguration _configuration = configuration;

  public byte[] GetPasswordHash(string password, byte[] passwordSalt)
  {
    string passwordSaltPlusString = _configuration.GetSection("AppSettings:PasswordKey").Value +
        Convert.ToBase64String(passwordSalt);

    return KeyDerivation.Pbkdf2(
        password: password,
        salt: Encoding.ASCII.GetBytes(passwordSaltPlusString),
        prf: KeyDerivationPrf.HMACSHA256,
        iterationCount: 100000,
        numBytesRequested: 256 / 8
    );
  }

  public bool VerifyPassword(byte[] passwordHashDb, byte[] passwordHash)
  {
    for (int i = 0; i < passwordHash.Length; i++)
    {
      if (passwordHash[i] != passwordHashDb[i]) return false;
    }

    return true;
  }

  public string CreateToken(User user)
  {
    Claim[] claims = [
            new Claim("userId", user.Id.ToString()),
            new Claim("username", user.Username ?? ""),
            new Claim("email", user.Email ?? ""),
            new Claim("firstName", user.FirstName ?? ""),
            new Claim("lastName", user.LastName ?? ""),
    ];

    string? tokenKeyString = _configuration.GetSection("AppSettings:TokenKey").Value;

    SymmetricSecurityKey key = new(Encoding.UTF8.GetBytes(tokenKeyString ?? ""));

    SigningCredentials creds = new(key, SecurityAlgorithms.HmacSha512Signature);

    SecurityTokenDescriptor tokenDescriptor = new()
    {
      Subject = new ClaimsIdentity(claims),
      Expires = DateTime.Now.AddDays(1),
      SigningCredentials = creds
    };

    JwtSecurityTokenHandler tokenHandler = new();

    SecurityToken token = tokenHandler.CreateToken(tokenDescriptor);

    return tokenHandler.WriteToken(token);
  }

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
}
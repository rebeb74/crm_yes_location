using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.IdentityModel.Tokens;
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

  public string CreateToken(int userId)
  {
    Claim[] claims = new[]
    {
                new Claim("userId", userId.ToString())
            };

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
}
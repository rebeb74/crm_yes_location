using YesLocation.Domain.Entities;

namespace YesLocation.Domain.Interfaces;

public interface IAuthService
{
  byte[] GetPasswordHash(string password, byte[] passwordSalt);
  bool VerifyPassword(byte[] passwordHashDb, byte[] passwordHash);
  string CreateToken(User user);
  bool ValidateEmail(string email);

}
namespace YesLocation.Domain.Interfaces;

public interface IAuthService
{
  byte[] GetPasswordHash(string password, byte[] passwordSalt);
  string CreateToken(int userId);
}
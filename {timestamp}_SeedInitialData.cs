using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.Extensions.Configuration;
using System.Security.Cryptography;
using YesLocation.Domain.Interfaces;

namespace YesLocation.Infrastructure.Migrations
{
  public partial class SeedInitialData(IConfiguration configuration, IAuthService authService) : Migration
  {
    private readonly IConfiguration _configuration = configuration;
    private readonly IAuthService _authService = authService;

    protected override void Up(MigrationBuilder migrationBuilder)
    {

      string defaultPassword = _configuration.GetSection("AppSettings:DefaultPassword").Value!;

      byte[] passwordSalt = _authService!.GetPasswordSalt();

      byte[] passwordHash = _authService!.GetPasswordHash(defaultPassword, passwordSalt);

      migrationBuilder.InsertData(
          table: "Users",
          columns: ["Id", "Username", "FirstName", "LastName", "Email", "CreatedAt", "UpdatedAt", "CreatedBy", "UpdatedBy"],
          values: [1, "bertrand", "Bertrand", "Petit", "bertrandpetit10@gmail.com", DateTime.UtcNow, DateTime.UtcNow, 1, 1]
      );

      migrationBuilder.InsertData(
          table: "Auth",
          columns: ["Id", "UserId", "PasswordHash", "PasswordSalt", "CreatedAt", "UpdatedAt", "CreatedBy", "UpdatedBy"],
          values: [1, 1, passwordHash, passwordSalt, DateTime.UtcNow, DateTime.UtcNow, 1, 1]
      );

      migrationBuilder.InsertData(
          table: "Roles",
          columns: ["Id", "Name", "Description", "Value", "CreatedAt", "UpdatedAt", "CreatedBy", "UpdatedBy"],
          values: [1, "Admin", "Administrator role", 7, DateTime.UtcNow, DateTime.UtcNow, 1, 1]
      );

      migrationBuilder.InsertData(
          table: "Roles",
          columns: ["Id", "Name", "Description", "Value", "CreatedAt", "UpdatedAt", "CreatedBy", "UpdatedBy"],
          values: [2, "User", "Basic user role", 3, DateTime.UtcNow, DateTime.UtcNow, 1, 1]
      );

      migrationBuilder.InsertData(
          table: "UserRoles",
          columns: ["UserId", "RoleId", "CreatedAt", "UpdatedAt", "CreatedBy", "UpdatedBy"],
          values: [1, 1, DateTime.UtcNow, DateTime.UtcNow, 1, 1]
      );

      migrationBuilder.InsertData(
          table: "UserRoles",
          columns: ["UserId", "RoleId", "CreatedAt", "UpdatedAt", "CreatedBy", "UpdatedBy"],
          values: [1, 2, DateTime.UtcNow, DateTime.UtcNow, 1, 1]
      );
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
      migrationBuilder.DeleteData(
          table: "Auth",
          keyColumn: "Id",
          keyValue: 1);

      migrationBuilder.DeleteData(
          table: "Users",
          keyColumn: "Id",
          keyValue: 1);

      migrationBuilder.DeleteData(
          table: "UserRoles",
          keyColumn: "UserId",
          keyValue: 1);

      migrationBuilder.DeleteData(
          table: "Roles",
          keyColumn: "Id",
          keyValue: 1);

      migrationBuilder.DeleteData(
          table: "Roles",
          keyColumn: "Id",
          keyValue: 2);
    }
  }
}

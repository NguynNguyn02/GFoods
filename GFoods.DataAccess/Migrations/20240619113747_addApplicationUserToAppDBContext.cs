using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GFoods.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class addApplicationUserToAppDBContext : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "City", "Coin", "CompanyId", "ConcurrencyStamp", "Discriminator", "Email", "EmailConfirmed", "LockoutEnabled", "LockoutEnd", "Name", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "PostalCode", "SecurityStamp", "State", "StreetAddress", "TwoFactorEnabled", "UserName" },
                values: new object[] { "fdf13906-e523-45b8-a093-a3b078762205", 0, "CityA", 10000.0, null, "5e8ddfdc-3f99-4090-9db3-a006d5d89639", "ApplicationUser", null, false, false, null, "Nguyen", null, null, null, null, false, "PostalCodeA", "c1b22d52-e488-47f0-9f57-14b9d07e9cde", "StateA", "StreetAddressA", false, null });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "fdf13906-e523-45b8-a093-a3b078762205");
        }
    }
}

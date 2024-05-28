using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GFoods.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class addAliasToDb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Alias",
                table: "Products",
                type: "nvarchar(250)",
                maxLength: 250,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Alias",
                table: "CategoryProducts",
                type: "nvarchar(250)",
                maxLength: 250,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Alias",
                table: "Categories",
                type: "nvarchar(250)",
                maxLength: 250,
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 1,
                column: "Alias",
                value: "nguyen4");

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 2,
                column: "Alias",
                value: "nguyen5");

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 3,
                column: "Alias",
                value: "nguyen6");

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 4,
                column: "Alias",
                value: "nguyen1");

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 5,
                column: "Alias",
                value: "nguyen2");

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 6,
                column: "Alias",
                value: "nguyen3");

            migrationBuilder.UpdateData(
                table: "CategoryProducts",
                keyColumn: "Id",
                keyValue: 1,
                column: "Alias",
                value: "nguyen3");

            migrationBuilder.UpdateData(
                table: "CategoryProducts",
                keyColumn: "Id",
                keyValue: 2,
                column: "Alias",
                value: "nguyen3");

            migrationBuilder.UpdateData(
                table: "CategoryProducts",
                keyColumn: "Id",
                keyValue: 3,
                column: "Alias",
                value: "nguyen3");

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 1,
                column: "Alias",
                value: "nguyen1");

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 2,
                column: "Alias",
                value: "nguyen2");

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 3,
                column: "Alias",
                value: "nguyen3");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Alias",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "Alias",
                table: "CategoryProducts");

            migrationBuilder.DropColumn(
                name: "Alias",
                table: "Categories");
        }
    }
}

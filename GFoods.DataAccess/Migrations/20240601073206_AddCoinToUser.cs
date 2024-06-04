using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GFoods.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class AddCoinToUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "Coin",
                table: "AspNetUsers",
                type: "float",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Coin",
                table: "AspNetUsers");
        }
    }
}

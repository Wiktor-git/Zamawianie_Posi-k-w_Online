using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ZamawianiePosiłkowOnline.Migrations
{
    /// <inheritdoc />
    public partial class citiesToRestaurantRename : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CityName",
                table: "Cities",
                newName: "RestaurantName");

            migrationBuilder.RenameColumn(
                name: "CityID",
                table: "Cities",
                newName: "RestaurantID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "RestaurantName",
                table: "Cities",
                newName: "CityName");

            migrationBuilder.RenameColumn(
                name: "RestaurantID",
                table: "Cities",
                newName: "CityID");
        }
    }
}

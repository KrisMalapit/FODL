using Microsoft.EntityFrameworkCore.Migrations;

namespace FODLSystem.Migrations
{
    public partial class _5 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "LubeTrucks",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "Equipments",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "Dispensers",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "Areas",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Status",
                table: "LubeTrucks");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "Equipments");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "Dispensers");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "Areas");
        }
    }
}

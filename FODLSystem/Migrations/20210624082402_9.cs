 using Microsoft.EntityFrameworkCore.Migrations;

namespace FODLSystem.Migrations
{
    public partial class _9 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "FuelOils",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "FuelOilDetails",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Status",
                table: "FuelOils");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "FuelOilDetails");
        }
    }
}

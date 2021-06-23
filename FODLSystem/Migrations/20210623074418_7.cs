using Microsoft.EntityFrameworkCore.Migrations;

namespace FODLSystem.Migrations
{
    public partial class _7 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "DepartmentCode",
                table: "Equipments",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FuelCodeDiesel",
                table: "Equipments",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FuelCodeOil",
                table: "Equipments",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DepartmentCode",
                table: "Equipments");

            migrationBuilder.DropColumn(
                name: "FuelCodeDiesel",
                table: "Equipments");

            migrationBuilder.DropColumn(
                name: "FuelCodeOil",
                table: "Equipments");
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

namespace FODLSystem.Migrations
{
    public partial class _16 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Dispensers",
                columns: new[] { "Id", "Name", "No", "Status" },
                values: new object[] { 1, "N/A", "na", "Default" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Dispensers",
                keyColumn: "Id",
                keyValue: 1);
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

namespace FODLSystem.Migrations
{
    public partial class _14 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "LubeTrucks",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_LubeTrucks_No_Status",
                table: "LubeTrucks",
                columns: new[] { "No", "Status" },
                unique: true,
                filter: "[Status] IS NOT NULL");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_LubeTrucks_No_Status",
                table: "LubeTrucks");

            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "LubeTrucks",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);
        }
    }
}

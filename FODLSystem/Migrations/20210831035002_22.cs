using Microsoft.EntityFrameworkCore.Migrations;

namespace FODLSystem.Migrations
{
    public partial class _22 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Users_Username_Status",
                table: "Users");

            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "Users",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_Username",
                table: "Users",
                column: "Username",
                unique: true,
                filter: "[Username] IS NOT NULL");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Users_Username",
                table: "Users");

            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "Users",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_Username_Status",
                table: "Users",
                columns: new[] { "Username", "Status" },
                unique: true,
                filter: "[Username] IS NOT NULL AND [Status] IS NOT NULL");
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

namespace FODLSystem.Migrations
{
    public partial class _12 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "FuelOils",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Shift",
                table: "FuelOils",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_FuelOils_TransactionDate_Shift_Status",
                table: "FuelOils",
                columns: new[] { "TransactionDate", "Shift", "Status" },
                unique: true,
                filter: "[Shift] IS NOT NULL AND [Status] IS NOT NULL");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_FuelOils_TransactionDate_Shift_Status",
                table: "FuelOils");

            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "FuelOils",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Shift",
                table: "FuelOils",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);
        }
    }
}

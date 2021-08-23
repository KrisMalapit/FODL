using Microsoft.EntityFrameworkCore.Migrations;

namespace FODLSystem.Migrations
{
    public partial class _18 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_FuelOils_TransactionDate_Shift_Status_LubeTruckId",
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

        protected override void Down(MigrationBuilder migrationBuilder)
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
                name: "IX_FuelOils_TransactionDate_Shift_Status_LubeTruckId",
                table: "FuelOils",
                columns: new[] { "TransactionDate", "Shift", "Status", "LubeTruckId" },
                unique: true,
                filter: "[Shift] IS NOT NULL AND [Status] IS NOT NULL");
        }
    }
}

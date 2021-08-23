using Microsoft.EntityFrameworkCore.Migrations;

namespace FODLSystem.Migrations
{
    public partial class _17 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_FuelOils_TransactionDate_Shift_Status",
                table: "FuelOils");

            migrationBuilder.CreateIndex(
                name: "IX_FuelOils_TransactionDate_Shift_Status_LubeTruckId",
                table: "FuelOils",
                columns: new[] { "TransactionDate", "Shift", "Status", "LubeTruckId" },
                unique: true,
                filter: "[Shift] IS NOT NULL AND [Status] IS NOT NULL");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_FuelOils_TransactionDate_Shift_Status_LubeTruckId",
                table: "FuelOils");

            migrationBuilder.CreateIndex(
                name: "IX_FuelOils_TransactionDate_Shift_Status",
                table: "FuelOils",
                columns: new[] { "TransactionDate", "Shift", "Status" },
                unique: true,
                filter: "[Shift] IS NOT NULL AND [Status] IS NOT NULL");
        }
    }
}

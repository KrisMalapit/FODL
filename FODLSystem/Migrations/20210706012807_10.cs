using Microsoft.EntityFrameworkCore.Migrations;

namespace FODLSystem.Migrations
{
    public partial class _10 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "DispenserId",
                table: "FuelOils",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "LubeTruckId",
                table: "FuelOils",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.InsertData(
                table: "Locations",
                columns: new[] { "Id", "List", "No", "OfficeCode", "Status" },
                values: new object[] { 1, "N/A", "na", "000", "Deleted" });

            migrationBuilder.InsertData(
                table: "LubeTrucks",
                columns: new[] { "Id", "Description", "No", "OldId", "Status" },
                values: new object[] { 1, "N/A", "na", "0", "Deleted" });

            migrationBuilder.CreateIndex(
                name: "IX_FuelOils_DispenserId",
                table: "FuelOils",
                column: "DispenserId");

            migrationBuilder.CreateIndex(
                name: "IX_FuelOils_LubeTruckId",
                table: "FuelOils",
                column: "LubeTruckId");

            migrationBuilder.AddForeignKey(
                name: "FK_FuelOils_Dispensers_DispenserId",
                table: "FuelOils",
                column: "DispenserId",
                principalTable: "Dispensers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_FuelOils_LubeTrucks_LubeTruckId",
                table: "FuelOils",
                column: "LubeTruckId",
                principalTable: "LubeTrucks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FuelOils_Dispensers_DispenserId",
                table: "FuelOils");

            migrationBuilder.DropForeignKey(
                name: "FK_FuelOils_LubeTrucks_LubeTruckId",
                table: "FuelOils");

            migrationBuilder.DropIndex(
                name: "IX_FuelOils_DispenserId",
                table: "FuelOils");

            migrationBuilder.DropIndex(
                name: "IX_FuelOils_LubeTruckId",
                table: "FuelOils");

            migrationBuilder.DeleteData(
                table: "Locations",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "LubeTrucks",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DropColumn(
                name: "DispenserId",
                table: "FuelOils");

            migrationBuilder.DropColumn(
                name: "LubeTruckId",
                table: "FuelOils");
        }
    }
}

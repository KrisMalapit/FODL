using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace FODLSystem.Migrations
{
    public partial class _30 : Migration
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

            migrationBuilder.InsertData(
                table: "Components",
                columns: new[] { "Id", "DateModified", "Name", "Status" },
                values: new object[] { 1, new DateTime(2021, 10, 28, 18, 5, 29, 221, DateTimeKind.Local), "N/A", "Default" });

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

            migrationBuilder.DeleteData(
                table: "Components",
                keyColumn: "Id",
                keyValue: 1);

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

using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace FODLSystem.Migrations
{
    public partial class _32 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_FuelOils_TransactionDate_Shift_Status_LubeTruckId_DispenserId",
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

            migrationBuilder.UpdateData(
                table: "Components",
                keyColumn: "Id",
                keyValue: 1,
                column: "DateModified",
                value: new DateTime(2021, 12, 7, 14, 27, 0, 211, DateTimeKind.Local));
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

            migrationBuilder.UpdateData(
                table: "Components",
                keyColumn: "Id",
                keyValue: 1,
                column: "DateModified",
                value: new DateTime(2021, 11, 10, 13, 34, 23, 343, DateTimeKind.Local));

            migrationBuilder.CreateIndex(
                name: "IX_FuelOils_TransactionDate_Shift_Status_LubeTruckId_DispenserId",
                table: "FuelOils",
                columns: new[] { "TransactionDate", "Shift", "Status", "LubeTruckId", "DispenserId" },
                unique: true,
                filter: "[Shift] IS NOT NULL AND [Status] IS NOT NULL");
        }
    }
}

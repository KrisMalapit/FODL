using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace FODLSystem.Migrations
{
    public partial class _31 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_FuelOils_TransactionDate_Shift_Status_LubeTruckId",
                table: "FuelOils");

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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_FuelOils_TransactionDate_Shift_Status_LubeTruckId_DispenserId",
                table: "FuelOils");

            migrationBuilder.UpdateData(
                table: "Components",
                keyColumn: "Id",
                keyValue: 1,
                column: "DateModified",
                value: new DateTime(2021, 10, 28, 18, 5, 29, 221, DateTimeKind.Local));

            migrationBuilder.CreateIndex(
                name: "IX_FuelOils_TransactionDate_Shift_Status_LubeTruckId",
                table: "FuelOils",
                columns: new[] { "TransactionDate", "Shift", "Status", "LubeTruckId" },
                unique: true,
                filter: "[Shift] IS NOT NULL AND [Status] IS NOT NULL");
        }
    }
}

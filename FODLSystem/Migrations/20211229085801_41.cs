using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace FODLSystem.Migrations
{
    public partial class _41 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FuelOils_Drivers_DriverId",
                table: "FuelOils");

            migrationBuilder.DropIndex(
                name: "IX_FuelOils_DriverId",
                table: "FuelOils");

            migrationBuilder.DropColumn(
                name: "DriverId",
                table: "FuelOils");

            migrationBuilder.AddColumn<int>(
                name: "DriverId",
                table: "FuelOilDetails",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Components",
                keyColumn: "Id",
                keyValue: 1,
                column: "DateModified",
                value: new DateTime(2021, 12, 29, 16, 58, 1, 609, DateTimeKind.Local));

            migrationBuilder.CreateIndex(
                name: "IX_FuelOilDetails_DriverId",
                table: "FuelOilDetails",
                column: "DriverId");

            migrationBuilder.AddForeignKey(
                name: "FK_FuelOilDetails_Drivers_DriverId",
                table: "FuelOilDetails",
                column: "DriverId",
                principalTable: "Drivers",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FuelOilDetails_Drivers_DriverId",
                table: "FuelOilDetails");

            migrationBuilder.DropIndex(
                name: "IX_FuelOilDetails_DriverId",
                table: "FuelOilDetails");

            migrationBuilder.DropColumn(
                name: "DriverId",
                table: "FuelOilDetails");

            migrationBuilder.AddColumn<int>(
                name: "DriverId",
                table: "FuelOils",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Components",
                keyColumn: "Id",
                keyValue: 1,
                column: "DateModified",
                value: new DateTime(2021, 12, 29, 15, 53, 36, 949, DateTimeKind.Local));

            migrationBuilder.CreateIndex(
                name: "IX_FuelOils_DriverId",
                table: "FuelOils",
                column: "DriverId");

            migrationBuilder.AddForeignKey(
                name: "FK_FuelOils_Drivers_DriverId",
                table: "FuelOils",
                column: "DriverId",
                principalTable: "Drivers",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }
    }
}

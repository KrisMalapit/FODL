using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace FODLSystem.Migrations
{
    public partial class _42 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FuelOilDetails_Drivers_DriverId",
                table: "FuelOilDetails");

            migrationBuilder.AlterColumn<int>(
                name: "DriverId",
                table: "FuelOilDetails",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.UpdateData(
                table: "Components",
                keyColumn: "Id",
                keyValue: 1,
                column: "DateModified",
                value: new DateTime(2022, 1, 4, 8, 57, 29, 546, DateTimeKind.Local));

            migrationBuilder.AddForeignKey(
                name: "FK_FuelOilDetails_Drivers_DriverId",
                table: "FuelOilDetails",
                column: "DriverId",
                principalTable: "Drivers",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FuelOilDetails_Drivers_DriverId",
                table: "FuelOilDetails");

            migrationBuilder.AlterColumn<int>(
                name: "DriverId",
                table: "FuelOilDetails",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.UpdateData(
                table: "Components",
                keyColumn: "Id",
                keyValue: 1,
                column: "DateModified",
                value: new DateTime(2021, 12, 29, 16, 58, 1, 609, DateTimeKind.Local));

            migrationBuilder.AddForeignKey(
                name: "FK_FuelOilDetails_Drivers_DriverId",
                table: "FuelOilDetails",
                column: "DriverId",
                principalTable: "Drivers",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }
    }
}

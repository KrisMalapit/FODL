using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace FODLSystem.Migrations
{
    public partial class _37 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "DriverId",
                table: "FuelOils",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Components",
                keyColumn: "Id",
                keyValue: 1,
                column: "DateModified",
                value: new DateTime(2021, 12, 29, 14, 54, 56, 72, DateTimeKind.Local));

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

        protected override void Down(MigrationBuilder migrationBuilder)
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

            migrationBuilder.UpdateData(
                table: "Components",
                keyColumn: "Id",
                keyValue: 1,
                column: "DateModified",
                value: new DateTime(2021, 12, 29, 13, 14, 28, 476, DateTimeKind.Local));
        }
    }
}

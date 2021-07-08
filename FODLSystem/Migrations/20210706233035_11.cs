using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace FODLSystem.Migrations
{
    public partial class _11 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "TransactionDate",
                table: "FuelOils",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.UpdateData(
                table: "Locations",
                keyColumn: "Id",
                keyValue: 1,
                column: "Status",
                value: "Default");

            migrationBuilder.UpdateData(
                table: "LubeTrucks",
                keyColumn: "Id",
                keyValue: 1,
                column: "Status",
                value: "Default");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TransactionDate",
                table: "FuelOils");

            migrationBuilder.UpdateData(
                table: "Locations",
                keyColumn: "Id",
                keyValue: 1,
                column: "Status",
                value: "Deleted");

            migrationBuilder.UpdateData(
                table: "LubeTrucks",
                keyColumn: "Id",
                keyValue: 1,
                column: "Status",
                value: "Deleted");
        }
    }
}

using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace FODLSystem.Migrations
{
    public partial class _43 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "BatchName",
                table: "FuelOils",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Components",
                keyColumn: "Id",
                keyValue: 1,
                column: "DateModified",
                value: new DateTime(2022, 1, 13, 13, 23, 52, 31, DateTimeKind.Local));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BatchName",
                table: "FuelOils");

            migrationBuilder.UpdateData(
                table: "Components",
                keyColumn: "Id",
                keyValue: 1,
                column: "DateModified",
                value: new DateTime(2022, 1, 4, 8, 57, 29, 546, DateTimeKind.Local));
        }
    }
}

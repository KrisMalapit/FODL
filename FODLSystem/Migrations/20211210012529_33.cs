using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace FODLSystem.Migrations
{
    public partial class _33 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "OriginalDate",
                table: "FuelOils",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.UpdateData(
                table: "Components",
                keyColumn: "Id",
                keyValue: 1,
                column: "DateModified",
                value: new DateTime(2021, 12, 10, 9, 25, 28, 802, DateTimeKind.Local));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OriginalDate",
                table: "FuelOils");

            migrationBuilder.UpdateData(
                table: "Components",
                keyColumn: "Id",
                keyValue: 1,
                column: "DateModified",
                value: new DateTime(2021, 12, 7, 14, 27, 0, 211, DateTimeKind.Local));
        }
    }
}

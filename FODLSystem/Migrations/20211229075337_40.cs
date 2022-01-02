using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace FODLSystem.Migrations
{
    public partial class _40 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Components",
                keyColumn: "Id",
                keyValue: 1,
                column: "DateModified",
                value: new DateTime(2021, 12, 29, 15, 53, 36, 949, DateTimeKind.Local));

            migrationBuilder.UpdateData(
                table: "Drivers",
                keyColumn: "ID",
                keyValue: 1,
                column: "DateModified",
                value: new DateTime(1900, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Components",
                keyColumn: "Id",
                keyValue: 1,
                column: "DateModified",
                value: new DateTime(2021, 12, 29, 15, 52, 5, 36, DateTimeKind.Local));

            migrationBuilder.UpdateData(
                table: "Drivers",
                keyColumn: "ID",
                keyValue: 1,
                column: "DateModified",
                value: null);
        }
    }
}

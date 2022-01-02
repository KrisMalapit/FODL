using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace FODLSystem.Migrations
{
    public partial class _39 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Components",
                keyColumn: "Id",
                keyValue: 1,
                column: "DateModified",
                value: new DateTime(2021, 12, 29, 15, 52, 5, 36, DateTimeKind.Local));

            migrationBuilder.InsertData(
                table: "Drivers",
                columns: new[] { "ID", "DateModified", "IdNumber", "Name", "Position", "Status" },
                values: new object[] { 1, null, "00000", "N/A", "N/A", "Enabled" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Drivers",
                keyColumn: "ID",
                keyValue: 1);

            migrationBuilder.UpdateData(
                table: "Components",
                keyColumn: "Id",
                keyValue: 1,
                column: "DateModified",
                value: new DateTime(2021, 12, 29, 14, 54, 56, 72, DateTimeKind.Local));
        }
    }
}

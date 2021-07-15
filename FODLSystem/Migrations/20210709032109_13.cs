using Microsoft.EntityFrameworkCore.Migrations;

namespace FODLSystem.Migrations
{
    public partial class _13 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "DispenserAccess",
                table: "Users",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LubeAccess",
                table: "Users",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DispenserAccess",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "LubeAccess",
                table: "Users");
        }
    }
}

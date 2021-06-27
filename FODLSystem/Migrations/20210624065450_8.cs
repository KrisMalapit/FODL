using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace FODLSystem.Migrations
{
    public partial class _8 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Areas");

            migrationBuilder.CreateTable(
                name: "Locations",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    No = table.Column<string>(type: "VARCHAR(20)", nullable: false),
                    List = table.Column<string>(type: "VARCHAR(100)", nullable: false),
                    OfficeCode = table.Column<string>(type: "VARCHAR(10)", nullable: false),
                    Status = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Locations", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FuelOils",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ReferenceNo = table.Column<string>(nullable: true),
                    Shift = table.Column<string>(nullable: true),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    CreatedBy = table.Column<string>(nullable: true),
                    EquipmentId = table.Column<int>(nullable: false),
                    LocationId = table.Column<int>(nullable: false),
                    SMR = table.Column<string>(nullable: true),
                    Signature = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FuelOils", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FuelOils_Equipments_EquipmentId",
                        column: x => x.EquipmentId,
                        principalTable: "Equipments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FuelOils_Locations_LocationId",
                        column: x => x.LocationId,
                        principalTable: "Locations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FuelOilDetails",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    TimeInput = table.Column<DateTime>(nullable: false),
                    ItemId = table.Column<int>(nullable: false),
                    ComponentId = table.Column<int>(nullable: false),
                    VolumeQty = table.Column<int>(nullable: false),
                    FuelOilId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FuelOilDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FuelOilDetails_Components_ComponentId",
                        column: x => x.ComponentId,
                        principalTable: "Components",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FuelOilDetails_FuelOils_FuelOilId",
                        column: x => x.FuelOilId,
                        principalTable: "FuelOils",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FuelOilDetails_Items_ItemId",
                        column: x => x.ItemId,
                        principalTable: "Items",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FuelOilDetails_ComponentId",
                table: "FuelOilDetails",
                column: "ComponentId");

            migrationBuilder.CreateIndex(
                name: "IX_FuelOilDetails_FuelOilId",
                table: "FuelOilDetails",
                column: "FuelOilId");

            migrationBuilder.CreateIndex(
                name: "IX_FuelOilDetails_ItemId",
                table: "FuelOilDetails",
                column: "ItemId");

            migrationBuilder.CreateIndex(
                name: "IX_FuelOils_EquipmentId",
                table: "FuelOils",
                column: "EquipmentId");

            migrationBuilder.CreateIndex(
                name: "IX_FuelOils_LocationId",
                table: "FuelOils",
                column: "LocationId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FuelOilDetails");

            migrationBuilder.DropTable(
                name: "FuelOils");

            migrationBuilder.DropTable(
                name: "Locations");

            migrationBuilder.CreateTable(
                name: "Areas",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    List = table.Column<string>(type: "VARCHAR(100)", nullable: false),
                    No = table.Column<string>(type: "VARCHAR(20)", nullable: false),
                    OfficeCode = table.Column<string>(type: "VARCHAR(10)", nullable: false),
                    Status = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Areas", x => x.Id);
                });
        }
    }
}

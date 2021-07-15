using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace FODLSystem.Migrations
{
    public partial class _15 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FuelOilDetails_Components_ComponentId",
                table: "FuelOilDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_FuelOilDetails_Items_ItemId",
                table: "FuelOilDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_FuelOils_Equipments_EquipmentId",
                table: "FuelOils");

            migrationBuilder.DropForeignKey(
                name: "FK_FuelOils_Locations_LocationId",
                table: "FuelOils");

            migrationBuilder.DropIndex(
                name: "IX_FuelOils_EquipmentId",
                table: "FuelOils");

            migrationBuilder.DropIndex(
                name: "IX_FuelOils_LocationId",
                table: "FuelOils");

            migrationBuilder.DropIndex(
                name: "IX_FuelOilDetails_ComponentId",
                table: "FuelOilDetails");

            migrationBuilder.DropColumn(
                name: "EquipmentId",
                table: "FuelOils");

            migrationBuilder.DropColumn(
                name: "LocationId",
                table: "FuelOils");

            migrationBuilder.DropColumn(
                name: "SMR",
                table: "FuelOils");

            migrationBuilder.DropColumn(
                name: "Signature",
                table: "FuelOils");

            migrationBuilder.DropColumn(
                name: "ComponentId",
                table: "FuelOilDetails");

            migrationBuilder.RenameColumn(
                name: "VolumeQty",
                table: "FuelOilDetails",
                newName: "LocationId");

            migrationBuilder.RenameColumn(
                name: "TimeInput",
                table: "FuelOilDetails",
                newName: "CreatedDate");

            migrationBuilder.RenameColumn(
                name: "ItemId",
                table: "FuelOilDetails",
                newName: "EquipmentId");

            migrationBuilder.RenameIndex(
                name: "IX_FuelOilDetails_ItemId",
                table: "FuelOilDetails",
                newName: "IX_FuelOilDetails_EquipmentId");

            migrationBuilder.AddColumn<string>(
                name: "SMR",
                table: "FuelOilDetails",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Signature",
                table: "FuelOilDetails",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "FuelOilSubDetails",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    TimeInput = table.Column<DateTime>(nullable: false),
                    ItemId = table.Column<int>(nullable: false),
                    ComponentId = table.Column<int>(nullable: false),
                    VolumeQty = table.Column<int>(nullable: false),
                    FuelOilDetailId = table.Column<int>(nullable: false),
                    Status = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FuelOilSubDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FuelOilSubDetails_Components_ComponentId",
                        column: x => x.ComponentId,
                        principalTable: "Components",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FuelOilSubDetails_FuelOilDetails_FuelOilDetailId",
                        column: x => x.FuelOilDetailId,
                        principalTable: "FuelOilDetails",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FuelOilSubDetails_Items_ItemId",
                        column: x => x.ItemId,
                        principalTable: "Items",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FuelOilDetails_LocationId",
                table: "FuelOilDetails",
                column: "LocationId");

            migrationBuilder.CreateIndex(
                name: "IX_FuelOilSubDetails_ComponentId",
                table: "FuelOilSubDetails",
                column: "ComponentId");

            migrationBuilder.CreateIndex(
                name: "IX_FuelOilSubDetails_FuelOilDetailId",
                table: "FuelOilSubDetails",
                column: "FuelOilDetailId");

            migrationBuilder.CreateIndex(
                name: "IX_FuelOilSubDetails_ItemId",
                table: "FuelOilSubDetails",
                column: "ItemId");

            migrationBuilder.AddForeignKey(
                name: "FK_FuelOilDetails_Equipments_EquipmentId",
                table: "FuelOilDetails",
                column: "EquipmentId",
                principalTable: "Equipments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_FuelOilDetails_Locations_LocationId",
                table: "FuelOilDetails",
                column: "LocationId",
                principalTable: "Locations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FuelOilDetails_Equipments_EquipmentId",
                table: "FuelOilDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_FuelOilDetails_Locations_LocationId",
                table: "FuelOilDetails");

            migrationBuilder.DropTable(
                name: "FuelOilSubDetails");

            migrationBuilder.DropIndex(
                name: "IX_FuelOilDetails_LocationId",
                table: "FuelOilDetails");

            migrationBuilder.DropColumn(
                name: "SMR",
                table: "FuelOilDetails");

            migrationBuilder.DropColumn(
                name: "Signature",
                table: "FuelOilDetails");

            migrationBuilder.RenameColumn(
                name: "LocationId",
                table: "FuelOilDetails",
                newName: "VolumeQty");

            migrationBuilder.RenameColumn(
                name: "EquipmentId",
                table: "FuelOilDetails",
                newName: "ItemId");

            migrationBuilder.RenameColumn(
                name: "CreatedDate",
                table: "FuelOilDetails",
                newName: "TimeInput");

            migrationBuilder.RenameIndex(
                name: "IX_FuelOilDetails_EquipmentId",
                table: "FuelOilDetails",
                newName: "IX_FuelOilDetails_ItemId");

            migrationBuilder.AddColumn<int>(
                name: "EquipmentId",
                table: "FuelOils",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "LocationId",
                table: "FuelOils",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "SMR",
                table: "FuelOils",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Signature",
                table: "FuelOils",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ComponentId",
                table: "FuelOilDetails",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_FuelOils_EquipmentId",
                table: "FuelOils",
                column: "EquipmentId");

            migrationBuilder.CreateIndex(
                name: "IX_FuelOils_LocationId",
                table: "FuelOils",
                column: "LocationId");

            migrationBuilder.CreateIndex(
                name: "IX_FuelOilDetails_ComponentId",
                table: "FuelOilDetails",
                column: "ComponentId");

            migrationBuilder.AddForeignKey(
                name: "FK_FuelOilDetails_Components_ComponentId",
                table: "FuelOilDetails",
                column: "ComponentId",
                principalTable: "Components",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_FuelOilDetails_Items_ItemId",
                table: "FuelOilDetails",
                column: "ItemId",
                principalTable: "Items",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_FuelOils_Equipments_EquipmentId",
                table: "FuelOils",
                column: "EquipmentId",
                principalTable: "Equipments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_FuelOils_Locations_LocationId",
                table: "FuelOils",
                column: "LocationId",
                principalTable: "Locations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

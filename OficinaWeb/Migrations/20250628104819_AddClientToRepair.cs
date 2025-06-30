using Microsoft.EntityFrameworkCore.Migrations;

namespace OficinaWeb.Migrations
{
    public partial class AddClientToRepair : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MechanicRepairAndServices_Mechanics_MechanicsId",
                table: "MechanicRepairAndServices");

            migrationBuilder.DropForeignKey(
                name: "FK_MechanicRepairAndServices_RepairsAndServices_RepairAndServicesId",
                table: "MechanicRepairAndServices");

            migrationBuilder.DropForeignKey(
                name: "FK_Part_RepairsAndServices_RepairAndServicesId",
                table: "Part");

            migrationBuilder.DropForeignKey(
                name: "FK_RepairsAndServices_Vehicles_VehicleId",
                table: "RepairsAndServices");

            migrationBuilder.DropForeignKey(
                name: "FK_Vehicles_Clients_ClientId",
                table: "Vehicles");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Part",
                table: "Part");

            migrationBuilder.RenameTable(
                name: "Part",
                newName: "Parts");

            migrationBuilder.RenameIndex(
                name: "IX_Part_RepairAndServicesId",
                table: "Parts",
                newName: "IX_Parts_RepairAndServicesId");

            migrationBuilder.AddColumn<int>(
                name: "ClientId",
                table: "RepairsAndServices",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Parts",
                table: "Parts",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_RepairsAndServices_ClientId",
                table: "RepairsAndServices",
                column: "ClientId");

            migrationBuilder.AddForeignKey(
                name: "FK_MechanicRepairAndServices_Mechanics_MechanicsId",
                table: "MechanicRepairAndServices",
                column: "MechanicsId",
                principalTable: "Mechanics",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_MechanicRepairAndServices_RepairsAndServices_RepairAndServicesId",
                table: "MechanicRepairAndServices",
                column: "RepairAndServicesId",
                principalTable: "RepairsAndServices",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Parts_RepairsAndServices_RepairAndServicesId",
                table: "Parts",
                column: "RepairAndServicesId",
                principalTable: "RepairsAndServices",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_RepairsAndServices_Clients_ClientId",
                table: "RepairsAndServices",
                column: "ClientId",
                principalTable: "Clients",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_RepairsAndServices_Vehicles_VehicleId",
                table: "RepairsAndServices",
                column: "VehicleId",
                principalTable: "Vehicles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Vehicles_Clients_ClientId",
                table: "Vehicles",
                column: "ClientId",
                principalTable: "Clients",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MechanicRepairAndServices_Mechanics_MechanicsId",
                table: "MechanicRepairAndServices");

            migrationBuilder.DropForeignKey(
                name: "FK_MechanicRepairAndServices_RepairsAndServices_RepairAndServicesId",
                table: "MechanicRepairAndServices");

            migrationBuilder.DropForeignKey(
                name: "FK_Parts_RepairsAndServices_RepairAndServicesId",
                table: "Parts");

            migrationBuilder.DropForeignKey(
                name: "FK_RepairsAndServices_Clients_ClientId",
                table: "RepairsAndServices");

            migrationBuilder.DropForeignKey(
                name: "FK_RepairsAndServices_Vehicles_VehicleId",
                table: "RepairsAndServices");

            migrationBuilder.DropForeignKey(
                name: "FK_Vehicles_Clients_ClientId",
                table: "Vehicles");

            migrationBuilder.DropIndex(
                name: "IX_RepairsAndServices_ClientId",
                table: "RepairsAndServices");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Parts",
                table: "Parts");

            migrationBuilder.DropColumn(
                name: "ClientId",
                table: "RepairsAndServices");

            migrationBuilder.RenameTable(
                name: "Parts",
                newName: "Part");

            migrationBuilder.RenameIndex(
                name: "IX_Parts_RepairAndServicesId",
                table: "Part",
                newName: "IX_Part_RepairAndServicesId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Part",
                table: "Part",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_MechanicRepairAndServices_Mechanics_MechanicsId",
                table: "MechanicRepairAndServices",
                column: "MechanicsId",
                principalTable: "Mechanics",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_MechanicRepairAndServices_RepairsAndServices_RepairAndServicesId",
                table: "MechanicRepairAndServices",
                column: "RepairAndServicesId",
                principalTable: "RepairsAndServices",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Part_RepairsAndServices_RepairAndServicesId",
                table: "Part",
                column: "RepairAndServicesId",
                principalTable: "RepairsAndServices",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_RepairsAndServices_Vehicles_VehicleId",
                table: "RepairsAndServices",
                column: "VehicleId",
                principalTable: "Vehicles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Vehicles_Clients_ClientId",
                table: "Vehicles",
                column: "ClientId",
                principalTable: "Clients",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

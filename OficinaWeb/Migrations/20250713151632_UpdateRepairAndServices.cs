using Microsoft.EntityFrameworkCore.Migrations;

namespace OficinaWeb.Migrations
{
    public partial class UpdateRepairAndServices : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ServiceType",
                table: "RepairsAndServices");

            migrationBuilder.AddColumn<int>(
                name: "ServiceTypeId",
                table: "RepairsAndServices",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_RepairsAndServices_ServiceTypeId",
                table: "RepairsAndServices",
                column: "ServiceTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_RepairsAndServices_ServiceTypes_ServiceTypeId",
                table: "RepairsAndServices",
                column: "ServiceTypeId",
                principalTable: "ServiceTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RepairsAndServices_ServiceTypes_ServiceTypeId",
                table: "RepairsAndServices");

            migrationBuilder.DropIndex(
                name: "IX_RepairsAndServices_ServiceTypeId",
                table: "RepairsAndServices");

            migrationBuilder.DropColumn(
                name: "ServiceTypeId",
                table: "RepairsAndServices");

            migrationBuilder.AddColumn<string>(
                name: "ServiceType",
                table: "RepairsAndServices",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}

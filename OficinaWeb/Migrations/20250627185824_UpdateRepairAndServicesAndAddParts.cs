using Microsoft.EntityFrameworkCore.Migrations;

namespace OficinaWeb.Migrations
{
    public partial class UpdateRepairAndServicesAndAddParts : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Details",
                table: "RepairsAndServices",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "ServicePrice",
                table: "RepairsAndServices",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.CreateTable(
                name: "MechanicRepairAndServices",
                columns: table => new
                {
                    MechanicsId = table.Column<int>(type: "int", nullable: false),
                    RepairAndServicesId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MechanicRepairAndServices", x => new { x.MechanicsId, x.RepairAndServicesId });
                    table.ForeignKey(
                        name: "FK_MechanicRepairAndServices_Mechanics_MechanicsId",
                        column: x => x.MechanicsId,
                        principalTable: "Mechanics",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MechanicRepairAndServices_RepairsAndServices_RepairAndServicesId",
                        column: x => x.RepairAndServicesId,
                        principalTable: "RepairsAndServices",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Part",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    RepairAndServicesId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Part", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Part_RepairsAndServices_RepairAndServicesId",
                        column: x => x.RepairAndServicesId,
                        principalTable: "RepairsAndServices",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MechanicRepairAndServices_RepairAndServicesId",
                table: "MechanicRepairAndServices",
                column: "RepairAndServicesId");

            migrationBuilder.CreateIndex(
                name: "IX_Part_RepairAndServicesId",
                table: "Part",
                column: "RepairAndServicesId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MechanicRepairAndServices");

            migrationBuilder.DropTable(
                name: "Part");

            migrationBuilder.DropColumn(
                name: "Details",
                table: "RepairsAndServices");

            migrationBuilder.DropColumn(
                name: "ServicePrice",
                table: "RepairsAndServices");
        }
    }
}

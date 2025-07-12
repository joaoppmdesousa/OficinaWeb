using Microsoft.EntityFrameworkCore.Migrations;

namespace OficinaWeb.Migrations
{
    public partial class AddBrandAndModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Brand",
                table: "Vehicles");

            migrationBuilder.DropColumn(
                name: "Model",
                table: "Vehicles");

            migrationBuilder.AddColumn<int>(
                name: "CarBrandId",
                table: "Vehicles",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "CarModelId",
                table: "Vehicles",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "CarBrand",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CarBrand", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CarModel",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    CarBrandId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CarModel", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CarModel_CarBrand_CarBrandId",
                        column: x => x.CarBrandId,
                        principalTable: "CarBrand",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Vehicles_CarBrandId",
                table: "Vehicles",
                column: "CarBrandId");

            migrationBuilder.CreateIndex(
                name: "IX_Vehicles_CarModelId",
                table: "Vehicles",
                column: "CarModelId");

            migrationBuilder.CreateIndex(
                name: "IX_CarModel_CarBrandId",
                table: "CarModel",
                column: "CarBrandId");

            migrationBuilder.AddForeignKey(
                name: "FK_Vehicles_CarBrand_CarBrandId",
                table: "Vehicles",
                column: "CarBrandId",
                principalTable: "CarBrand",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Vehicles_CarModel_CarModelId",
                table: "Vehicles",
                column: "CarModelId",
                principalTable: "CarModel",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Vehicles_CarBrand_CarBrandId",
                table: "Vehicles");

            migrationBuilder.DropForeignKey(
                name: "FK_Vehicles_CarModel_CarModelId",
                table: "Vehicles");

            migrationBuilder.DropTable(
                name: "CarModel");

            migrationBuilder.DropTable(
                name: "CarBrand");

            migrationBuilder.DropIndex(
                name: "IX_Vehicles_CarBrandId",
                table: "Vehicles");

            migrationBuilder.DropIndex(
                name: "IX_Vehicles_CarModelId",
                table: "Vehicles");

            migrationBuilder.DropColumn(
                name: "CarBrandId",
                table: "Vehicles");

            migrationBuilder.DropColumn(
                name: "CarModelId",
                table: "Vehicles");

            migrationBuilder.AddColumn<string>(
                name: "Brand",
                table: "Vehicles",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Model",
                table: "Vehicles",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "");
        }
    }
}

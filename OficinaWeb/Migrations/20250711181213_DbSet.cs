using Microsoft.EntityFrameworkCore.Migrations;

namespace OficinaWeb.Migrations
{
    public partial class DbSet : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CarModel_CarBrand_CarBrandId",
                table: "CarModel");

            migrationBuilder.DropForeignKey(
                name: "FK_Vehicles_CarModel_CarModelId",
                table: "Vehicles");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CarModel",
                table: "CarModel");

            migrationBuilder.RenameTable(
                name: "CarModel",
                newName: "CarModels");

            migrationBuilder.RenameIndex(
                name: "IX_CarModel_CarBrandId",
                table: "CarModels",
                newName: "IX_CarModels_CarBrandId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CarModels",
                table: "CarModels",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_CarModels_CarBrand_CarBrandId",
                table: "CarModels",
                column: "CarBrandId",
                principalTable: "CarBrand",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Vehicles_CarModels_CarModelId",
                table: "Vehicles",
                column: "CarModelId",
                principalTable: "CarModels",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CarModels_CarBrand_CarBrandId",
                table: "CarModels");

            migrationBuilder.DropForeignKey(
                name: "FK_Vehicles_CarModels_CarModelId",
                table: "Vehicles");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CarModels",
                table: "CarModels");

            migrationBuilder.RenameTable(
                name: "CarModels",
                newName: "CarModel");

            migrationBuilder.RenameIndex(
                name: "IX_CarModels_CarBrandId",
                table: "CarModel",
                newName: "IX_CarModel_CarBrandId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CarModel",
                table: "CarModel",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_CarModel_CarBrand_CarBrandId",
                table: "CarModel",
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
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

namespace OficinaWeb.Migrations
{
    public partial class AddMechanicSpecialtyToMechanic : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Specialty",
                table: "Mechanics");

            migrationBuilder.AddColumn<int>(
                name: "MechanicSpecialtyId",
                table: "Mechanics",
                type: "int",
                maxLength: 30,
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Mechanics_MechanicSpecialtyId",
                table: "Mechanics",
                column: "MechanicSpecialtyId");

            migrationBuilder.AddForeignKey(
                name: "FK_Mechanics_Specialties_MechanicSpecialtyId",
                table: "Mechanics",
                column: "MechanicSpecialtyId",
                principalTable: "Specialties",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Mechanics_Specialties_MechanicSpecialtyId",
                table: "Mechanics");

            migrationBuilder.DropIndex(
                name: "IX_Mechanics_MechanicSpecialtyId",
                table: "Mechanics");

            migrationBuilder.DropColumn(
                name: "MechanicSpecialtyId",
                table: "Mechanics");

            migrationBuilder.AddColumn<string>(
                name: "Specialty",
                table: "Mechanics",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: false,
                defaultValue: "");
        }
    }
}

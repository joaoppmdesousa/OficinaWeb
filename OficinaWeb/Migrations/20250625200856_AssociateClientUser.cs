using Microsoft.EntityFrameworkCore.Migrations;

namespace OficinaWeb.Migrations
{
    public partial class AssociateClientUser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Clients_AspNetUsers_userId",
                table: "Clients");

            migrationBuilder.DropForeignKey(
                name: "FK_Mechanics_AspNetUsers_userId",
                table: "Mechanics");

            migrationBuilder.DropForeignKey(
                name: "FK_Vehicles_AspNetUsers_userId",
                table: "Vehicles");

            migrationBuilder.DropIndex(
                name: "IX_Clients_userId",
                table: "Clients");

            migrationBuilder.DropColumn(
                name: "userId",
                table: "Clients");

            migrationBuilder.RenameColumn(
                name: "userId",
                table: "Vehicles",
                newName: "UserId");

            migrationBuilder.RenameIndex(
                name: "IX_Vehicles_userId",
                table: "Vehicles",
                newName: "IX_Vehicles_UserId");

            migrationBuilder.RenameColumn(
                name: "userId",
                table: "Mechanics",
                newName: "UserId");

            migrationBuilder.RenameIndex(
                name: "IX_Mechanics_userId",
                table: "Mechanics",
                newName: "IX_Mechanics_UserId");

            migrationBuilder.AddColumn<int>(
                name: "ClientId",
                table: "AspNetUsers",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_ClientId",
                table: "AspNetUsers",
                column: "ClientId",
                unique: true,
                filter: "[ClientId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Clients_ClientId",
                table: "AspNetUsers",
                column: "ClientId",
                principalTable: "Clients",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Mechanics_AspNetUsers_UserId",
                table: "Mechanics",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Vehicles_AspNetUsers_UserId",
                table: "Vehicles",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Clients_ClientId",
                table: "AspNetUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_Mechanics_AspNetUsers_UserId",
                table: "Mechanics");

            migrationBuilder.DropForeignKey(
                name: "FK_Vehicles_AspNetUsers_UserId",
                table: "Vehicles");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_ClientId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "ClientId",
                table: "AspNetUsers");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "Vehicles",
                newName: "userId");

            migrationBuilder.RenameIndex(
                name: "IX_Vehicles_UserId",
                table: "Vehicles",
                newName: "IX_Vehicles_userId");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "Mechanics",
                newName: "userId");

            migrationBuilder.RenameIndex(
                name: "IX_Mechanics_UserId",
                table: "Mechanics",
                newName: "IX_Mechanics_userId");

            migrationBuilder.AddColumn<string>(
                name: "userId",
                table: "Clients",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Clients_userId",
                table: "Clients",
                column: "userId");

            migrationBuilder.AddForeignKey(
                name: "FK_Clients_AspNetUsers_userId",
                table: "Clients",
                column: "userId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Mechanics_AspNetUsers_userId",
                table: "Mechanics",
                column: "userId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Vehicles_AspNetUsers_userId",
                table: "Vehicles",
                column: "userId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}

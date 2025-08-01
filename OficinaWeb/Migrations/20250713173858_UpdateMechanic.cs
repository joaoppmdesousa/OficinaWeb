﻿using Microsoft.EntityFrameworkCore.Migrations;

namespace OficinaWeb.Migrations
{
    public partial class UpdateMechanic : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "Mechanics",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Email",
                table: "Mechanics");
        }
    }
}

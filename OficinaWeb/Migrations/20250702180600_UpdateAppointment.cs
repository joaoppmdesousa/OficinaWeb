using Microsoft.EntityFrameworkCore.Migrations;

namespace OficinaWeb.Migrations
{
    public partial class UpdateAppointment : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AppointmentType",
                table: "Appointment",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AppointmentType",
                table: "Appointment");
        }
    }
}

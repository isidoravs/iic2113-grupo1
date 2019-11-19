using Microsoft.EntityFrameworkCore.Migrations;

namespace ConferenceApp.Data.Migrations
{
    public partial class AddEventCentreIdToRooms : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Rooms_EventCentre_EventCentreId",
                table: "Rooms");

            migrationBuilder.AlterColumn<int>(
                name: "EventCentreId",
                table: "Rooms",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Rooms_EventCentre_EventCentreId",
                table: "Rooms",
                column: "EventCentreId",
                principalTable: "EventCentre",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Rooms_EventCentre_EventCentreId",
                table: "Rooms");

            migrationBuilder.AlterColumn<int>(
                name: "EventCentreId",
                table: "Rooms",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddForeignKey(
                name: "FK_Rooms_EventCentre_EventCentreId",
                table: "Rooms",
                column: "EventCentreId",
                principalTable: "EventCentre",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}

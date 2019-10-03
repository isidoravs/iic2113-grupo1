using Microsoft.EntityFrameworkCore.Migrations;

namespace ConferenceApp.Data.Migrations
{
    public partial class AddRoomsToEventCentre : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "EventCentreId",
                table: "Rooms",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Rooms_EventCentreId",
                table: "Rooms",
                column: "EventCentreId");

            migrationBuilder.AddForeignKey(
                name: "FK_Rooms_EventCentre_EventCentreId",
                table: "Rooms",
                column: "EventCentreId",
                principalTable: "EventCentre",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Rooms_EventCentre_EventCentreId",
                table: "Rooms");

            migrationBuilder.DropIndex(
                name: "IX_Rooms_EventCentreId",
                table: "Rooms");

            migrationBuilder.DropColumn(
                name: "EventCentreId",
                table: "Rooms");
        }
    }
}

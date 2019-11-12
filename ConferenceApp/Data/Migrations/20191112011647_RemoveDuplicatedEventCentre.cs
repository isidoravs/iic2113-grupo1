using Microsoft.EntityFrameworkCore.Migrations;

namespace ConferenceApp.Data.Migrations
{
    public partial class RemoveDuplicatedEventCentre : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ConferenceVersions_EventCentre_EventCentreId",
                table: "ConferenceVersions");

            migrationBuilder.DropForeignKey(
                name: "FK_Rooms_EventCentre_EventCentreId",
                table: "Rooms");

            migrationBuilder.DropPrimaryKey(
                name: "PK_EventCentre",
                table: "EventCentre");

            migrationBuilder.RenameTable(
                name: "EventCentre",
                newName: "EventCentres");

            migrationBuilder.AddPrimaryKey(
                name: "PK_EventCentres",
                table: "EventCentres",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ConferenceVersions_EventCentres_EventCentreId",
                table: "ConferenceVersions",
                column: "EventCentreId",
                principalTable: "EventCentres",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Rooms_EventCentres_EventCentreId",
                table: "Rooms",
                column: "EventCentreId",
                principalTable: "EventCentres",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ConferenceVersions_EventCentres_EventCentreId",
                table: "ConferenceVersions");

            migrationBuilder.DropForeignKey(
                name: "FK_Rooms_EventCentres_EventCentreId",
                table: "Rooms");

            migrationBuilder.DropPrimaryKey(
                name: "PK_EventCentres",
                table: "EventCentres");

            migrationBuilder.RenameTable(
                name: "EventCentres",
                newName: "EventCentre");

            migrationBuilder.AddPrimaryKey(
                name: "PK_EventCentre",
                table: "EventCentre",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ConferenceVersions_EventCentre_EventCentreId",
                table: "ConferenceVersions",
                column: "EventCentreId",
                principalTable: "EventCentre",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Rooms_EventCentre_EventCentreId",
                table: "Rooms",
                column: "EventCentreId",
                principalTable: "EventCentre",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

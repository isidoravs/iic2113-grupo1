using Microsoft.EntityFrameworkCore.Migrations;

namespace ConferenceApp.Data.Migrations
{
    public partial class AddEventCentreIdToConferenceVersion : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ConferenceVersions_EventCentre_EventCentreId",
                table: "ConferenceVersions");

            migrationBuilder.AlterColumn<int>(
                name: "EventCentreId",
                table: "ConferenceVersions",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_ConferenceVersions_EventCentre_EventCentreId",
                table: "ConferenceVersions",
                column: "EventCentreId",
                principalTable: "EventCentre",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ConferenceVersions_EventCentre_EventCentreId",
                table: "ConferenceVersions");

            migrationBuilder.AlterColumn<int>(
                name: "EventCentreId",
                table: "ConferenceVersions",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddForeignKey(
                name: "FK_ConferenceVersions_EventCentre_EventCentreId",
                table: "ConferenceVersions",
                column: "EventCentreId",
                principalTable: "EventCentre",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}

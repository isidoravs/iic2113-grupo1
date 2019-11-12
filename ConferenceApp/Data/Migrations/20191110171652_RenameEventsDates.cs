using Microsoft.EntityFrameworkCore.Migrations;

namespace ConferenceApp.Data.Migrations
{
    public partial class RenameEventsDates : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ConferenceVersions_EventCentre_EventCentreId",
                table: "ConferenceVersions");

            migrationBuilder.DropForeignKey(
                name: "FK_Rooms_EventCentre_EventCentreId",
                table: "Rooms");

            migrationBuilder.RenameColumn(
                name: "startDate",
                table: "Events",
                newName: "StartDate");

            migrationBuilder.RenameColumn(
                name: "endDate",
                table: "Events",
                newName: "EndDate");

            migrationBuilder.AlterColumn<int>(
                name: "EventCentreId",
                table: "Rooms",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

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
                name: "FK_ConferenceVersions_EventCentre_EventCentreId",
                table: "ConferenceVersions");

            migrationBuilder.DropForeignKey(
                name: "FK_Rooms_EventCentre_EventCentreId",
                table: "Rooms");

            migrationBuilder.RenameColumn(
                name: "StartDate",
                table: "Events",
                newName: "startDate");

            migrationBuilder.RenameColumn(
                name: "EndDate",
                table: "Events",
                newName: "endDate");

            migrationBuilder.AlterColumn<int>(
                name: "EventCentreId",
                table: "Rooms",
                nullable: true,
                oldClrType: typeof(int));

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

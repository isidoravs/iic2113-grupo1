using Microsoft.EntityFrameworkCore.Migrations;

namespace ConferenceApp.Data.Migrations
{
    public partial class AddVersionsToConferences : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "startDate",
                table: "ConferenceVersions",
                newName: "StartDate");

            migrationBuilder.RenameColumn(
                name: "number",
                table: "ConferenceVersions",
                newName: "Number");

            migrationBuilder.RenameColumn(
                name: "endDate",
                table: "ConferenceVersions",
                newName: "EndDate");

            migrationBuilder.AddColumn<int>(
                name: "ConferenceId",
                table: "ConferenceVersions",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ConferenceVersions_ConferenceId",
                table: "ConferenceVersions",
                column: "ConferenceId");

            migrationBuilder.AddForeignKey(
                name: "FK_ConferenceVersions_Conferences_ConferenceId",
                table: "ConferenceVersions",
                column: "ConferenceId",
                principalTable: "Conferences",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ConferenceVersions_Conferences_ConferenceId",
                table: "ConferenceVersions");

            migrationBuilder.DropIndex(
                name: "IX_ConferenceVersions_ConferenceId",
                table: "ConferenceVersions");

            migrationBuilder.DropColumn(
                name: "ConferenceId",
                table: "ConferenceVersions");

            migrationBuilder.RenameColumn(
                name: "StartDate",
                table: "ConferenceVersions",
                newName: "startDate");

            migrationBuilder.RenameColumn(
                name: "Number",
                table: "ConferenceVersions",
                newName: "number");

            migrationBuilder.RenameColumn(
                name: "EndDate",
                table: "ConferenceVersions",
                newName: "endDate");
        }
    }
}

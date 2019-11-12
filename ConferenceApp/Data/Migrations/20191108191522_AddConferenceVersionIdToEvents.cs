using Microsoft.EntityFrameworkCore.Migrations;

namespace ConferenceApp.Data.Migrations
{
    public partial class AddConferenceVersionIdToEvents : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ConferenceVersionId",
                table: "Events",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Events_ConferenceVersionId",
                table: "Events",
                column: "ConferenceVersionId");

            migrationBuilder.AddForeignKey(
                name: "FK_Events_ConferenceVersions_ConferenceVersionId",
                table: "Events",
                column: "ConferenceVersionId",
                principalTable: "ConferenceVersions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Events_ConferenceVersions_ConferenceVersionId",
                table: "Events");

            migrationBuilder.DropIndex(
                name: "IX_Events_ConferenceVersionId",
                table: "Events");

            migrationBuilder.DropColumn(
                name: "ConferenceVersionId",
                table: "Events");
        }
    }
}

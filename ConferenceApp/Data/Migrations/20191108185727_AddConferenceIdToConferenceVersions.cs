using Microsoft.EntityFrameworkCore.Migrations;

namespace ConferenceApp.Data.Migrations
{
    public partial class AddConferenceIdToConferenceVersions : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ConferenceVersions_Conferences_ConferenceId",
                table: "ConferenceVersions");

            migrationBuilder.AlterColumn<int>(
                name: "ConferenceId",
                table: "ConferenceVersions",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_ConferenceVersions_Conferences_ConferenceId",
                table: "ConferenceVersions",
                column: "ConferenceId",
                principalTable: "Conferences",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ConferenceVersions_Conferences_ConferenceId",
                table: "ConferenceVersions");

            migrationBuilder.AlterColumn<int>(
                name: "ConferenceId",
                table: "ConferenceVersions",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddForeignKey(
                name: "FK_ConferenceVersions_Conferences_ConferenceId",
                table: "ConferenceVersions",
                column: "ConferenceId",
                principalTable: "Conferences",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}

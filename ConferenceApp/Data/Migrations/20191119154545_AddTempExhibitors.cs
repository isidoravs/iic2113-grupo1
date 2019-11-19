using Microsoft.EntityFrameworkCore.Migrations;

namespace ConferenceApp.Data.Migrations
{
    public partial class AddTempExhibitors : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Moderator",
                table: "Events",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Panelists",
                table: "Events",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Exhibitor",
                table: "Events",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Talk_Exhibitor",
                table: "Events",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Moderator",
                table: "Events");

            migrationBuilder.DropColumn(
                name: "Panelists",
                table: "Events");

            migrationBuilder.DropColumn(
                name: "Exhibitor",
                table: "Events");

            migrationBuilder.DropColumn(
                name: "Talk_Exhibitor",
                table: "Events");
        }
    }
}

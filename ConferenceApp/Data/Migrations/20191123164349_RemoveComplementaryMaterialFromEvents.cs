using Microsoft.EntityFrameworkCore.Migrations;

namespace ConferenceApp.Data.Migrations
{
    public partial class RemoveComplementaryMaterialFromEvents : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ComplementaryMaterial",
                table: "Events");

            migrationBuilder.DropColumn(
                name: "Talk_ComplementaryMaterial",
                table: "Events");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ComplementaryMaterial",
                table: "Events",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Talk_ComplementaryMaterial",
                table: "Events",
                nullable: true);
        }
    }
}

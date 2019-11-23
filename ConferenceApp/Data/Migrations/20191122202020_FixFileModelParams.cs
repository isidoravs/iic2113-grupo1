using Microsoft.EntityFrameworkCore.Migrations;

namespace ConferenceApp.Data.Migrations
{
    public partial class FixFileModelParams : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "FileDescription",
                table: "FileViewModels",
                newName: "Description");

            migrationBuilder.AddColumn<int>(
                name: "FileId",
                table: "Events",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FileId",
                table: "Events");

            migrationBuilder.RenameColumn(
                name: "Description",
                table: "FileViewModels",
                newName: "FileDescription");
        }
    }
}

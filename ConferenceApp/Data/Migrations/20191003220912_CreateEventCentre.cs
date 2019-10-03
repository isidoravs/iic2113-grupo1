using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace ConferenceApp.Data.Migrations
{
    public partial class CreateEventCentre : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "EventCentreId",
                table: "ConferenceVersions",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "EventCentre",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    Name = table.Column<string>(nullable: false),
                    Location = table.Column<string>(nullable: false),
                    MapImage = table.Column<string>(nullable: true),
                    Latitude = table.Column<float>(nullable: false),
                    Longitude = table.Column<float>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EventCentre", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ConferenceVersions_EventCentreId",
                table: "ConferenceVersions",
                column: "EventCentreId");

            migrationBuilder.AddForeignKey(
                name: "FK_ConferenceVersions_EventCentre_EventCentreId",
                table: "ConferenceVersions",
                column: "EventCentreId",
                principalTable: "EventCentre",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ConferenceVersions_EventCentre_EventCentreId",
                table: "ConferenceVersions");

            migrationBuilder.DropTable(
                name: "EventCentre");

            migrationBuilder.DropIndex(
                name: "IX_ConferenceVersions_EventCentreId",
                table: "ConferenceVersions");

            migrationBuilder.DropColumn(
                name: "EventCentreId",
                table: "ConferenceVersions");
        }
    }
}

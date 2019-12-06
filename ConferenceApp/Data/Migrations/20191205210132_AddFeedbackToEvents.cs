using Microsoft.EntityFrameworkCore.Migrations;

namespace ConferenceApp.Data.Migrations
{
    public partial class AddFeedbackToEvents : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "EventId",
                table: "Feedbacks",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Feedbacks_EventId",
                table: "Feedbacks",
                column: "EventId");

            migrationBuilder.AddForeignKey(
                name: "FK_Feedbacks_Events_EventId",
                table: "Feedbacks",
                column: "EventId",
                principalTable: "Events",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Feedbacks_Events_EventId",
                table: "Feedbacks");

            migrationBuilder.DropIndex(
                name: "IX_Feedbacks_EventId",
                table: "Feedbacks");

            migrationBuilder.DropColumn(
                name: "EventId",
                table: "Feedbacks");
        }
    }
}

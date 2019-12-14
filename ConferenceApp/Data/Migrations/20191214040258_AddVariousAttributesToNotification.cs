using Microsoft.EntityFrameworkCore.Migrations;

namespace ConferenceApp.Data.Migrations
{
    public partial class AddVariousAttributesToNotification : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SenderId",
                table: "Notifications");

            migrationBuilder.AddColumn<int>(
                name: "ConferenceId",
                table: "Notifications",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "EventId",
                table: "Notifications",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsEventNotification",
                table: "Notifications",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Seen",
                table: "Notifications",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "Subject",
                table: "Notifications",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Notifications_ConferenceId",
                table: "Notifications",
                column: "ConferenceId");

            migrationBuilder.CreateIndex(
                name: "IX_Notifications_EventId",
                table: "Notifications",
                column: "EventId");

            migrationBuilder.AddForeignKey(
                name: "FK_Notifications_Conferences_ConferenceId",
                table: "Notifications",
                column: "ConferenceId",
                principalTable: "Conferences",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Notifications_Events_EventId",
                table: "Notifications",
                column: "EventId",
                principalTable: "Events",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Notifications_Conferences_ConferenceId",
                table: "Notifications");

            migrationBuilder.DropForeignKey(
                name: "FK_Notifications_Events_EventId",
                table: "Notifications");

            migrationBuilder.DropIndex(
                name: "IX_Notifications_ConferenceId",
                table: "Notifications");

            migrationBuilder.DropIndex(
                name: "IX_Notifications_EventId",
                table: "Notifications");

            migrationBuilder.DropColumn(
                name: "ConferenceId",
                table: "Notifications");

            migrationBuilder.DropColumn(
                name: "EventId",
                table: "Notifications");

            migrationBuilder.DropColumn(
                name: "IsEventNotification",
                table: "Notifications");

            migrationBuilder.DropColumn(
                name: "Seen",
                table: "Notifications");

            migrationBuilder.DropColumn(
                name: "Subject",
                table: "Notifications");

            migrationBuilder.AddColumn<string>(
                name: "SenderId",
                table: "Notifications",
                nullable: false,
                defaultValue: "");
        }
    }
}

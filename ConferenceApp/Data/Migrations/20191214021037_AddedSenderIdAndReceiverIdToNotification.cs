using Microsoft.EntityFrameworkCore.Migrations;

namespace ConferenceApp.Data.Migrations
{
    public partial class AddedSenderIdAndReceiverIdToNotification : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ReceiverId",
                table: "Notifications",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "SenderId",
                table: "Notifications",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ReceiverId",
                table: "Notifications");

            migrationBuilder.DropColumn(
                name: "SenderId",
                table: "Notifications");
        }
    }
}

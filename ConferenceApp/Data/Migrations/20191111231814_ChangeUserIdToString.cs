using Microsoft.EntityFrameworkCore.Migrations;

namespace ConferenceApp.Data.Migrations
{
    public partial class ChangeUserIdToString : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Role_AspNetUsers_UserId1",
                table: "Role");

            migrationBuilder.DropIndex(
                name: "IX_Role_UserId1",
                table: "Role");

            migrationBuilder.DropColumn(
                name: "UserId1",
                table: "Role");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "Role",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.CreateIndex(
                name: "IX_Role_UserId",
                table: "Role",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Role_AspNetUsers_UserId",
                table: "Role",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Role_AspNetUsers_UserId",
                table: "Role");

            migrationBuilder.DropIndex(
                name: "IX_Role_UserId",
                table: "Role");

            migrationBuilder.AlterColumn<int>(
                name: "UserId",
                table: "Role",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UserId1",
                table: "Role",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Role_UserId1",
                table: "Role",
                column: "UserId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Role_AspNetUsers_UserId1",
                table: "Role",
                column: "UserId1",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}

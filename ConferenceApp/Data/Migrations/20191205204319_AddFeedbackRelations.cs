using Microsoft.EntityFrameworkCore.Migrations;

namespace ConferenceApp.Data.Migrations
{
    public partial class AddFeedbackRelations : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FeedbackScopes_FeedbackCategories_FeedbackCategoryId",
                table: "FeedbackScopes");

            migrationBuilder.DropForeignKey(
                name: "FK_FeedbackScopes_Feedbacks_FeedbackId",
                table: "FeedbackScopes");

            migrationBuilder.AlterColumn<int>(
                name: "FeedbackId",
                table: "FeedbackScopes",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "FeedbackCategoryId",
                table: "FeedbackScopes",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_FeedbackScopes_FeedbackCategories_FeedbackCategoryId",
                table: "FeedbackScopes",
                column: "FeedbackCategoryId",
                principalTable: "FeedbackCategories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_FeedbackScopes_Feedbacks_FeedbackId",
                table: "FeedbackScopes",
                column: "FeedbackId",
                principalTable: "Feedbacks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FeedbackScopes_FeedbackCategories_FeedbackCategoryId",
                table: "FeedbackScopes");

            migrationBuilder.DropForeignKey(
                name: "FK_FeedbackScopes_Feedbacks_FeedbackId",
                table: "FeedbackScopes");

            migrationBuilder.AlterColumn<int>(
                name: "FeedbackId",
                table: "FeedbackScopes",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AlterColumn<int>(
                name: "FeedbackCategoryId",
                table: "FeedbackScopes",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddForeignKey(
                name: "FK_FeedbackScopes_FeedbackCategories_FeedbackCategoryId",
                table: "FeedbackScopes",
                column: "FeedbackCategoryId",
                principalTable: "FeedbackCategories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_FeedbackScopes_Feedbacks_FeedbackId",
                table: "FeedbackScopes",
                column: "FeedbackId",
                principalTable: "Feedbacks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}

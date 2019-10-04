using Microsoft.EntityFrameworkCore.Migrations;

namespace ConferenceApp.Data.Migrations
{
    public partial class FixFeedbackCategoryRelations : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FeedbackCategories_FeedbackScopes_FeedbackScopeId",
                table: "FeedbackCategories");

            migrationBuilder.DropIndex(
                name: "IX_FeedbackCategories_FeedbackScopeId",
                table: "FeedbackCategories");

            migrationBuilder.DropColumn(
                name: "FeedbackScopeId",
                table: "FeedbackCategories");

            migrationBuilder.AddColumn<int>(
                name: "FeedbackCategoryId",
                table: "FeedbackScopes",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_FeedbackScopes_FeedbackCategoryId",
                table: "FeedbackScopes",
                column: "FeedbackCategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_FeedbackScopes_FeedbackCategories_FeedbackCategoryId",
                table: "FeedbackScopes",
                column: "FeedbackCategoryId",
                principalTable: "FeedbackCategories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FeedbackScopes_FeedbackCategories_FeedbackCategoryId",
                table: "FeedbackScopes");

            migrationBuilder.DropIndex(
                name: "IX_FeedbackScopes_FeedbackCategoryId",
                table: "FeedbackScopes");

            migrationBuilder.DropColumn(
                name: "FeedbackCategoryId",
                table: "FeedbackScopes");

            migrationBuilder.AddColumn<int>(
                name: "FeedbackScopeId",
                table: "FeedbackCategories",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_FeedbackCategories_FeedbackScopeId",
                table: "FeedbackCategories",
                column: "FeedbackScopeId");

            migrationBuilder.AddForeignKey(
                name: "FK_FeedbackCategories_FeedbackScopes_FeedbackScopeId",
                table: "FeedbackCategories",
                column: "FeedbackScopeId",
                principalTable: "FeedbackScopes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}

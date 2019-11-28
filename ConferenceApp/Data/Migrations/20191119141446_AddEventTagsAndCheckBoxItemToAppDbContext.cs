using Microsoft.EntityFrameworkCore.Migrations;

namespace ConferenceApp.Data.Migrations
{
    public partial class AddEventTagsAndCheckBoxItemToAppDbContext : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CheckBoxItem_Events_EventId",
                table: "CheckBoxItem");

            migrationBuilder.DropForeignKey(
                name: "FK_EventTag_Events_ChatId",
                table: "EventTag");

            migrationBuilder.DropForeignKey(
                name: "FK_EventTag_Events_EventId",
                table: "EventTag");

            migrationBuilder.DropForeignKey(
                name: "FK_EventTag_Events_PracticalSessionId",
                table: "EventTag");

            migrationBuilder.DropForeignKey(
                name: "FK_EventTag_Tags_TagId",
                table: "EventTag");

            migrationBuilder.DropForeignKey(
                name: "FK_EventTag_Events_TalkId",
                table: "EventTag");

            migrationBuilder.DropPrimaryKey(
                name: "PK_EventTag",
                table: "EventTag");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CheckBoxItem",
                table: "CheckBoxItem");

            migrationBuilder.RenameTable(
                name: "EventTag",
                newName: "EventTags");

            migrationBuilder.RenameTable(
                name: "CheckBoxItem",
                newName: "CheckBoxItems");

            migrationBuilder.RenameIndex(
                name: "IX_EventTag_TalkId",
                table: "EventTags",
                newName: "IX_EventTags_TalkId");

            migrationBuilder.RenameIndex(
                name: "IX_EventTag_TagId",
                table: "EventTags",
                newName: "IX_EventTags_TagId");

            migrationBuilder.RenameIndex(
                name: "IX_EventTag_PracticalSessionId",
                table: "EventTags",
                newName: "IX_EventTags_PracticalSessionId");

            migrationBuilder.RenameIndex(
                name: "IX_EventTag_EventId",
                table: "EventTags",
                newName: "IX_EventTags_EventId");

            migrationBuilder.RenameIndex(
                name: "IX_EventTag_ChatId",
                table: "EventTags",
                newName: "IX_EventTags_ChatId");

            migrationBuilder.RenameIndex(
                name: "IX_CheckBoxItem_EventId",
                table: "CheckBoxItems",
                newName: "IX_CheckBoxItems_EventId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_EventTags",
                table: "EventTags",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CheckBoxItems",
                table: "CheckBoxItems",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_CheckBoxItems_Events_EventId",
                table: "CheckBoxItems",
                column: "EventId",
                principalTable: "Events",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_EventTags_Events_ChatId",
                table: "EventTags",
                column: "ChatId",
                principalTable: "Events",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_EventTags_Events_EventId",
                table: "EventTags",
                column: "EventId",
                principalTable: "Events",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_EventTags_Events_PracticalSessionId",
                table: "EventTags",
                column: "PracticalSessionId",
                principalTable: "Events",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_EventTags_Tags_TagId",
                table: "EventTags",
                column: "TagId",
                principalTable: "Tags",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_EventTags_Events_TalkId",
                table: "EventTags",
                column: "TalkId",
                principalTable: "Events",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CheckBoxItems_Events_EventId",
                table: "CheckBoxItems");

            migrationBuilder.DropForeignKey(
                name: "FK_EventTags_Events_ChatId",
                table: "EventTags");

            migrationBuilder.DropForeignKey(
                name: "FK_EventTags_Events_EventId",
                table: "EventTags");

            migrationBuilder.DropForeignKey(
                name: "FK_EventTags_Events_PracticalSessionId",
                table: "EventTags");

            migrationBuilder.DropForeignKey(
                name: "FK_EventTags_Tags_TagId",
                table: "EventTags");

            migrationBuilder.DropForeignKey(
                name: "FK_EventTags_Events_TalkId",
                table: "EventTags");

            migrationBuilder.DropPrimaryKey(
                name: "PK_EventTags",
                table: "EventTags");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CheckBoxItems",
                table: "CheckBoxItems");

            migrationBuilder.RenameTable(
                name: "EventTags",
                newName: "EventTag");

            migrationBuilder.RenameTable(
                name: "CheckBoxItems",
                newName: "CheckBoxItem");

            migrationBuilder.RenameIndex(
                name: "IX_EventTags_TalkId",
                table: "EventTag",
                newName: "IX_EventTag_TalkId");

            migrationBuilder.RenameIndex(
                name: "IX_EventTags_TagId",
                table: "EventTag",
                newName: "IX_EventTag_TagId");

            migrationBuilder.RenameIndex(
                name: "IX_EventTags_PracticalSessionId",
                table: "EventTag",
                newName: "IX_EventTag_PracticalSessionId");

            migrationBuilder.RenameIndex(
                name: "IX_EventTags_EventId",
                table: "EventTag",
                newName: "IX_EventTag_EventId");

            migrationBuilder.RenameIndex(
                name: "IX_EventTags_ChatId",
                table: "EventTag",
                newName: "IX_EventTag_ChatId");

            migrationBuilder.RenameIndex(
                name: "IX_CheckBoxItems_EventId",
                table: "CheckBoxItem",
                newName: "IX_CheckBoxItem_EventId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_EventTag",
                table: "EventTag",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CheckBoxItem",
                table: "CheckBoxItem",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_CheckBoxItem_Events_EventId",
                table: "CheckBoxItem",
                column: "EventId",
                principalTable: "Events",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_EventTag_Events_ChatId",
                table: "EventTag",
                column: "ChatId",
                principalTable: "Events",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_EventTag_Events_EventId",
                table: "EventTag",
                column: "EventId",
                principalTable: "Events",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_EventTag_Events_PracticalSessionId",
                table: "EventTag",
                column: "PracticalSessionId",
                principalTable: "Events",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_EventTag_Tags_TagId",
                table: "EventTag",
                column: "TagId",
                principalTable: "Tags",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_EventTag_Events_TalkId",
                table: "EventTag",
                column: "TalkId",
                principalTable: "Events",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}

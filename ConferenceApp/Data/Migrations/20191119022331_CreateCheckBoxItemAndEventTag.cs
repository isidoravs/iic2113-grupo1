using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace ConferenceApp.Data.Migrations
{
    public partial class CreateCheckBoxItemAndEventTag : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CheckBoxItem",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    Title = table.Column<string>(nullable: false),
                    IsChecked = table.Column<bool>(nullable: false),
                    EventId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CheckBoxItem", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CheckBoxItem_Events_EventId",
                        column: x => x.EventId,
                        principalTable: "Events",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "EventTag",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    EventId = table.Column<int>(nullable: false),
                    TagId = table.Column<int>(nullable: false),
                    ChatId = table.Column<int>(nullable: true),
                    PracticalSessionId = table.Column<int>(nullable: true),
                    TalkId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EventTag", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EventTag_Events_ChatId",
                        column: x => x.ChatId,
                        principalTable: "Events",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_EventTag_Events_EventId",
                        column: x => x.EventId,
                        principalTable: "Events",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EventTag_Events_PracticalSessionId",
                        column: x => x.PracticalSessionId,
                        principalTable: "Events",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_EventTag_Tags_TagId",
                        column: x => x.TagId,
                        principalTable: "Tags",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EventTag_Events_TalkId",
                        column: x => x.TalkId,
                        principalTable: "Events",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CheckBoxItem_EventId",
                table: "CheckBoxItem",
                column: "EventId");

            migrationBuilder.CreateIndex(
                name: "IX_EventTag_ChatId",
                table: "EventTag",
                column: "ChatId");

            migrationBuilder.CreateIndex(
                name: "IX_EventTag_EventId",
                table: "EventTag",
                column: "EventId");

            migrationBuilder.CreateIndex(
                name: "IX_EventTag_PracticalSessionId",
                table: "EventTag",
                column: "PracticalSessionId");

            migrationBuilder.CreateIndex(
                name: "IX_EventTag_TagId",
                table: "EventTag",
                column: "TagId");

            migrationBuilder.CreateIndex(
                name: "IX_EventTag_TalkId",
                table: "EventTag",
                column: "TalkId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CheckBoxItem");

            migrationBuilder.DropTable(
                name: "EventTag");
        }
    }
}

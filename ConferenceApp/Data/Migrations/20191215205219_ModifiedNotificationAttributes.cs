﻿using Microsoft.EntityFrameworkCore.Migrations;

namespace ConferenceApp.Data.Migrations
{
    public partial class FixNotificationAttributes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "EventId",
                table: "Notifications",
                nullable: true,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "ConferenceId",
                table: "Notifications",
                nullable: true,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ConferenceName",
                table: "Notifications",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "EventName",
                table: "Notifications",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SenderId",
                table: "Notifications",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ConferenceName",
                table: "Notifications");

            migrationBuilder.DropColumn(
                name: "EventName",
                table: "Notifications");

            migrationBuilder.DropColumn(
                name: "SenderId",
                table: "Notifications");

            migrationBuilder.AlterColumn<int>(
                name: "EventId",
                table: "Notifications",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AlterColumn<int>(
                name: "ConferenceId",
                table: "Notifications",
                nullable: true,
                oldClrType: typeof(int));
        }
    }
}
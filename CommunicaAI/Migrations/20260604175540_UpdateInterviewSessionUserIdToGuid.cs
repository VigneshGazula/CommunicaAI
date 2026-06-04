using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CommunicaAI.Migrations
{
    /// <inheritdoc />
    public partial class UpdateInterviewSessionUserIdToGuid : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Manually convert UserId from integer to uuid using raw SQL
            migrationBuilder.Sql(@"
                ALTER TABLE ""InterviewSessions"" 
                ALTER COLUMN ""UserId"" TYPE uuid 
                USING ""UserId""::text::uuid;
            ");

            migrationBuilder.AlterColumn<string>(
                name: "Topic",
                table: "InterviewSessions",
                type: "character varying(200)",
                maxLength: 200,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "InterviewSessions",
                type: "character varying(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "Role",
                table: "InterviewSessions",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "Difficulty",
                table: "InterviewSessions",
                type: "character varying(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.CreateIndex(
                name: "IX_InterviewSessions_UserId",
                table: "InterviewSessions",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_InterviewSessions_UserId",
                table: "InterviewSessions");

            // Manually convert UserId from uuid to integer using raw SQL
            migrationBuilder.Sql(@"
                ALTER TABLE ""InterviewSessions"" 
                ALTER COLUMN ""UserId"" TYPE integer 
                USING (""UserId""::text::integer);
            ");

            migrationBuilder.AlterColumn<string>(
                name: "Topic",
                table: "InterviewSessions",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(200)",
                oldMaxLength: 200);

            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "InterviewSessions",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(50)",
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<string>(
                name: "Role",
                table: "InterviewSessions",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(100)",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<string>(
                name: "Difficulty",
                table: "InterviewSessions",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(50)",
                oldMaxLength: 50);
        }
    }
}

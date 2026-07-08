using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CommunicaAI.Migrations
{
    /// <inheritdoc />
    public partial class AddInterviewTypeToInterviewSession : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "InterviewType",
                table: "InterviewSessions",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "InterviewType",
                table: "InterviewSessions");
        }
    }
}

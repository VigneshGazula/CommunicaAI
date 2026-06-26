using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CommunicaAI.Migrations
{
    /// <inheritdoc />
    public partial class UpdateInterviewResultModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CommunicationScore",
                table: "InterviewResults",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ConfidenceScore",
                table: "InterviewResults",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "OverallScore",
                table: "InterviewResults",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Recommendations",
                table: "InterviewResults",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Strengths",
                table: "InterviewResults",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Summary",
                table: "InterviewResults",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "TechnicalScore",
                table: "InterviewResults",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Weaknesses",
                table: "InterviewResults",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CommunicationScore",
                table: "InterviewResults");

            migrationBuilder.DropColumn(
                name: "ConfidenceScore",
                table: "InterviewResults");

            migrationBuilder.DropColumn(
                name: "OverallScore",
                table: "InterviewResults");

            migrationBuilder.DropColumn(
                name: "Recommendations",
                table: "InterviewResults");

            migrationBuilder.DropColumn(
                name: "Strengths",
                table: "InterviewResults");

            migrationBuilder.DropColumn(
                name: "Summary",
                table: "InterviewResults");

            migrationBuilder.DropColumn(
                name: "TechnicalScore",
                table: "InterviewResults");

            migrationBuilder.DropColumn(
                name: "Weaknesses",
                table: "InterviewResults");
        }
    }
}

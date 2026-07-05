using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CommunicaAI.Migrations
{
    /// <inheritdoc />
    public partial class AddVideoIntelligenceScores : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "EyeContactScore",
                table: "InterviewResults",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "PostureScore",
                table: "InterviewResults",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "FacialExpressionScore",
                table: "InterviewResults",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "VideoConfidenceScore",
                table: "InterviewResults",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "VideoFeedback",
                table: "InterviewResults",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EyeContactScore",
                table: "InterviewResults");

            migrationBuilder.DropColumn(
                name: "PostureScore",
                table: "InterviewResults");

            migrationBuilder.DropColumn(
                name: "FacialExpressionScore",
                table: "InterviewResults");

            migrationBuilder.DropColumn(
                name: "VideoConfidenceScore",
                table: "InterviewResults");

            migrationBuilder.DropColumn(
                name: "VideoFeedback",
                table: "InterviewResults");
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CommunicaAI.Migrations
{
    /// <inheritdoc />
    public partial class AddAICoachingFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CoachingSummary",
                table: "InterviewResults",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "CoachingStrengths",
                table: "InterviewResults",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "CoachingWeaknesses",
                table: "InterviewResults",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "CommunicationImprovements",
                table: "InterviewResults",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "TechnicalImprovements",
                table: "InterviewResults",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "VideoImprovements",
                table: "InterviewResults",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "VoiceImprovements",
                table: "InterviewResults",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "PracticeRecommendations",
                table: "InterviewResults",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "SuggestedRole",
                table: "InterviewResults",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "SuggestedDifficulty",
                table: "InterviewResults",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "SuggestedQuestionCount",
                table: "InterviewResults",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "LearningResources",
                table: "InterviewResults",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "MotivationalMessage",
                table: "InterviewResults",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CoachingSummary",
                table: "InterviewResults");

            migrationBuilder.DropColumn(
                name: "CoachingStrengths",
                table: "InterviewResults");

            migrationBuilder.DropColumn(
                name: "CoachingWeaknesses",
                table: "InterviewResults");

            migrationBuilder.DropColumn(
                name: "CommunicationImprovements",
                table: "InterviewResults");

            migrationBuilder.DropColumn(
                name: "TechnicalImprovements",
                table: "InterviewResults");

            migrationBuilder.DropColumn(
                name: "VideoImprovements",
                table: "InterviewResults");

            migrationBuilder.DropColumn(
                name: "VoiceImprovements",
                table: "InterviewResults");

            migrationBuilder.DropColumn(
                name: "PracticeRecommendations",
                table: "InterviewResults");

            migrationBuilder.DropColumn(
                name: "SuggestedRole",
                table: "InterviewResults");

            migrationBuilder.DropColumn(
                name: "SuggestedDifficulty",
                table: "InterviewResults");

            migrationBuilder.DropColumn(
                name: "SuggestedQuestionCount",
                table: "InterviewResults");

            migrationBuilder.DropColumn(
                name: "LearningResources",
                table: "InterviewResults");

            migrationBuilder.DropColumn(
                name: "MotivationalMessage",
                table: "InterviewResults");
        }
    }
}

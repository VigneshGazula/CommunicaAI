using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CommunicaAI.Migrations
{
    /// <inheritdoc />
    public partial class AddCompanyIntelligenceModule : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "CompanyProfileId",
                table: "InterviewSessions",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CoachingStrengths",
                table: "InterviewResults",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "CoachingSummary",
                table: "InterviewResults",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "CoachingWeaknesses",
                table: "InterviewResults",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "CommunicationAlignment",
                table: "InterviewResults",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "CommunicationImprovements",
                table: "InterviewResults",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "CompanyReadinessScore",
                table: "InterviewResults",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "CompanySpecificFeedback",
                table: "InterviewResults",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "CultureFit",
                table: "InterviewResults",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "EyeContactScore",
                table: "InterviewResults",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "FacialExpressionScore",
                table: "InterviewResults",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "LearningResources",
                table: "InterviewResults",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "MotivationalMessage",
                table: "InterviewResults",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "PostureScore",
                table: "InterviewResults",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "PracticeRecommendations",
                table: "InterviewResults",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "SuggestedDifficulty",
                table: "InterviewResults",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "SuggestedQuestionCount",
                table: "InterviewResults",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "SuggestedRole",
                table: "InterviewResults",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "TechnicalAlignment",
                table: "InterviewResults",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "TechnicalImprovements",
                table: "InterviewResults",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "VideoConfidenceScore",
                table: "InterviewResults",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "VideoFeedback",
                table: "InterviewResults",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "VideoImprovements",
                table: "InterviewResults",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "VoiceImprovements",
                table: "InterviewResults",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "AnswerStructureScore",
                table: "AnswerEvaluations",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "CommunicationScore",
                table: "AnswerEvaluations",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ConcisenessScore",
                table: "AnswerEvaluations",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ConfidenceScore",
                table: "AnswerEvaluations",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "GrammarScore",
                table: "AnswerEvaluations",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "PersuasivenessScore",
                table: "AnswerEvaluations",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ProfessionalismScore",
                table: "AnswerEvaluations",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "VocabularyScore",
                table: "AnswerEvaluations",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "CompanyProfiles",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CompanyName = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    InterviewStyle = table.Column<string>(type: "text", nullable: false),
                    FocusAreas = table.Column<string>(type: "text", nullable: false),
                    BehavioralExpectations = table.Column<string>(type: "text", nullable: false),
                    TechnicalExpectations = table.Column<string>(type: "text", nullable: false),
                    CommunicationExpectations = table.Column<string>(type: "text", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CompanyProfiles", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CompanyProfiles");

            migrationBuilder.DropColumn(
                name: "CompanyProfileId",
                table: "InterviewSessions");

            migrationBuilder.DropColumn(
                name: "CoachingStrengths",
                table: "InterviewResults");

            migrationBuilder.DropColumn(
                name: "CoachingSummary",
                table: "InterviewResults");

            migrationBuilder.DropColumn(
                name: "CoachingWeaknesses",
                table: "InterviewResults");

            migrationBuilder.DropColumn(
                name: "CommunicationAlignment",
                table: "InterviewResults");

            migrationBuilder.DropColumn(
                name: "CommunicationImprovements",
                table: "InterviewResults");

            migrationBuilder.DropColumn(
                name: "CompanyReadinessScore",
                table: "InterviewResults");

            migrationBuilder.DropColumn(
                name: "CompanySpecificFeedback",
                table: "InterviewResults");

            migrationBuilder.DropColumn(
                name: "CultureFit",
                table: "InterviewResults");

            migrationBuilder.DropColumn(
                name: "EyeContactScore",
                table: "InterviewResults");

            migrationBuilder.DropColumn(
                name: "FacialExpressionScore",
                table: "InterviewResults");

            migrationBuilder.DropColumn(
                name: "LearningResources",
                table: "InterviewResults");

            migrationBuilder.DropColumn(
                name: "MotivationalMessage",
                table: "InterviewResults");

            migrationBuilder.DropColumn(
                name: "PostureScore",
                table: "InterviewResults");

            migrationBuilder.DropColumn(
                name: "PracticeRecommendations",
                table: "InterviewResults");

            migrationBuilder.DropColumn(
                name: "SuggestedDifficulty",
                table: "InterviewResults");

            migrationBuilder.DropColumn(
                name: "SuggestedQuestionCount",
                table: "InterviewResults");

            migrationBuilder.DropColumn(
                name: "SuggestedRole",
                table: "InterviewResults");

            migrationBuilder.DropColumn(
                name: "TechnicalAlignment",
                table: "InterviewResults");

            migrationBuilder.DropColumn(
                name: "TechnicalImprovements",
                table: "InterviewResults");

            migrationBuilder.DropColumn(
                name: "VideoConfidenceScore",
                table: "InterviewResults");

            migrationBuilder.DropColumn(
                name: "VideoFeedback",
                table: "InterviewResults");

            migrationBuilder.DropColumn(
                name: "VideoImprovements",
                table: "InterviewResults");

            migrationBuilder.DropColumn(
                name: "VoiceImprovements",
                table: "InterviewResults");

            migrationBuilder.DropColumn(
                name: "AnswerStructureScore",
                table: "AnswerEvaluations");

            migrationBuilder.DropColumn(
                name: "CommunicationScore",
                table: "AnswerEvaluations");

            migrationBuilder.DropColumn(
                name: "ConcisenessScore",
                table: "AnswerEvaluations");

            migrationBuilder.DropColumn(
                name: "ConfidenceScore",
                table: "AnswerEvaluations");

            migrationBuilder.DropColumn(
                name: "GrammarScore",
                table: "AnswerEvaluations");

            migrationBuilder.DropColumn(
                name: "PersuasivenessScore",
                table: "AnswerEvaluations");

            migrationBuilder.DropColumn(
                name: "ProfessionalismScore",
                table: "AnswerEvaluations");

            migrationBuilder.DropColumn(
                name: "VocabularyScore",
                table: "AnswerEvaluations");
        }
    }
}

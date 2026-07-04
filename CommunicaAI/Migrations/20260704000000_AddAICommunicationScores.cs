using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CommunicaAI.Migrations
{
    /// <inheritdoc />
    public partial class AddAICommunicationScores : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CommunicationScore",
                table: "AnswerEvaluations",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ConfidenceScore",
                table: "AnswerEvaluations",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "GrammarScore",
                table: "AnswerEvaluations",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "VocabularyScore",
                table: "AnswerEvaluations",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ProfessionalismScore",
                table: "AnswerEvaluations",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "AnswerStructureScore",
                table: "AnswerEvaluations",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "PersuasivenessScore",
                table: "AnswerEvaluations",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ConcisenessScore",
                table: "AnswerEvaluations",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CommunicationScore",
                table: "AnswerEvaluations");

            migrationBuilder.DropColumn(
                name: "ConfidenceScore",
                table: "AnswerEvaluations");

            migrationBuilder.DropColumn(
                name: "GrammarScore",
                table: "AnswerEvaluations");

            migrationBuilder.DropColumn(
                name: "VocabularyScore",
                table: "AnswerEvaluations");

            migrationBuilder.DropColumn(
                name: "ProfessionalismScore",
                table: "AnswerEvaluations");

            migrationBuilder.DropColumn(
                name: "AnswerStructureScore",
                table: "AnswerEvaluations");

            migrationBuilder.DropColumn(
                name: "PersuasivenessScore",
                table: "AnswerEvaluations");

            migrationBuilder.DropColumn(
                name: "ConcisenessScore",
                table: "AnswerEvaluations");
        }
    }
}

using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CommunicaAI.Migrations
{
    /// <inheritdoc />
    public partial class AddedMigrationForNewAuth : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserMediaProfiles_Users_UserId",
                table: "UserMediaProfiles");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserMediaProfiles",
                table: "UserMediaProfiles");

            migrationBuilder.RenameTable(
                name: "UserMediaProfiles",
                newName: "UserMediaProfile");

            migrationBuilder.RenameIndex(
                name: "IX_UserMediaProfiles_UserId",
                table: "UserMediaProfile",
                newName: "IX_UserMediaProfile_UserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserMediaProfile",
                table: "UserMediaProfile",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "UserVerificationProfiles",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    EnrollmentAudioUrl = table.Column<string>(type: "text", nullable: false),
                    EnrollmentAudioPublicId = table.Column<string>(type: "text", nullable: false),
                    EnrollmentVideoUrl = table.Column<string>(type: "text", nullable: false),
                    EnrollmentVideoPublicId = table.Column<string>(type: "text", nullable: false),
                    EnrolledAtUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAtUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserVerificationProfiles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserVerificationProfiles_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserVerificationProfiles_UserId",
                table: "UserVerificationProfiles",
                column: "UserId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_UserMediaProfile_Users_UserId",
                table: "UserMediaProfile",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserMediaProfile_Users_UserId",
                table: "UserMediaProfile");

            migrationBuilder.DropTable(
                name: "UserVerificationProfiles");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserMediaProfile",
                table: "UserMediaProfile");

            migrationBuilder.RenameTable(
                name: "UserMediaProfile",
                newName: "UserMediaProfiles");

            migrationBuilder.RenameIndex(
                name: "IX_UserMediaProfile_UserId",
                table: "UserMediaProfiles",
                newName: "IX_UserMediaProfiles_UserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserMediaProfiles",
                table: "UserMediaProfiles",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_UserMediaProfiles_Users_UserId",
                table: "UserMediaProfiles",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BlazorQuizWASM.Server.Data.Migrations
{
    /// <inheritdoc />
    public partial class NicknameUsers : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Nickname",
                table: "AspNetUsers",
                type: "nvarchar(15)",
                maxLength: 15,
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "MediaType",
                columns: table => new
                {
                    MediaId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Mediatype = table.Column<string>(type: "nvarchar(15)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MediaType", x => x.MediaId);
                });

            migrationBuilder.CreateTable(
                name: "MediaFile",
                columns: table => new
                {
                    MediaFileId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FkMediaTypeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    MediaFileName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FileExtension = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FileSizeInBytes = table.Column<long>(type: "bigint", nullable: false),
                    FilePath = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MediaFile", x => x.MediaFileId);
                    table.ForeignKey(
                        name: "FK_MediaFile_MediaType_FkMediaTypeId",
                        column: x => x.FkMediaTypeId,
                        principalTable: "MediaType",
                        principalColumn: "MediaId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Question",
                columns: table => new
                {
                    QuestionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FkUserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    FkFileId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Question", x => x.QuestionId);
                    table.ForeignKey(
                        name: "FK_Question_AspNetUsers_FkUserId",
                        column: x => x.FkUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Question_MediaFile_FkFileId",
                        column: x => x.FkFileId,
                        principalTable: "MediaFile",
                        principalColumn: "MediaFileId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Answer",
                columns: table => new
                {
                    AnswerId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FkQuestionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsCorrect = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Answer", x => x.AnswerId);
                    table.ForeignKey(
                        name: "FK_Answer_Question_FkQuestionId",
                        column: x => x.FkQuestionId,
                        principalTable: "Question",
                        principalColumn: "QuestionId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "QuizItem",
                columns: table => new
                {
                    QuizItemId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FkUserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    FkQuestionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IsScored = table.Column<bool>(type: "bit", nullable: false),
                    TimeLimit = table.Column<int>(type: "int", nullable: false),
                    Started_At = table.Column<DateTime>(type: "datetime2", nullable: false),
                    QuestionsQuestionId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QuizItem", x => x.QuizItemId);
                    table.ForeignKey(
                        name: "FK_QuizItem_AspNetUsers_FkUserId",
                        column: x => x.FkUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_QuizItem_Question_QuestionsQuestionId",
                        column: x => x.QuestionsQuestionId,
                        principalTable: "Question",
                        principalColumn: "QuestionId");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Answer_FkQuestionId",
                table: "Answer",
                column: "FkQuestionId");

            migrationBuilder.CreateIndex(
                name: "IX_MediaFile_FkMediaTypeId",
                table: "MediaFile",
                column: "FkMediaTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Question_FkFileId",
                table: "Question",
                column: "FkFileId");

            migrationBuilder.CreateIndex(
                name: "IX_Question_FkUserId",
                table: "Question",
                column: "FkUserId");

            migrationBuilder.CreateIndex(
                name: "IX_QuizItem_FkUserId",
                table: "QuizItem",
                column: "FkUserId");

            migrationBuilder.CreateIndex(
                name: "IX_QuizItem_QuestionsQuestionId",
                table: "QuizItem",
                column: "QuestionsQuestionId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Answer");

            migrationBuilder.DropTable(
                name: "QuizItem");

            migrationBuilder.DropTable(
                name: "Question");

            migrationBuilder.DropTable(
                name: "MediaFile");

            migrationBuilder.DropTable(
                name: "MediaType");

            migrationBuilder.DropColumn(
                name: "Nickname",
                table: "AspNetUsers");
        }
    }
}

using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BlazorQuizWASM.Server.Data.Migrations
{
    /// <inheritdoc />
    public partial class DomainEntitiesCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Answer_Question_FkQuestionId",
                table: "Answer");

            migrationBuilder.DropForeignKey(
                name: "FK_MediaFile_MediaType_FkMediaTypeId",
                table: "MediaFile");

            migrationBuilder.DropForeignKey(
                name: "FK_Question_AspNetUsers_FkUserId",
                table: "Question");

            migrationBuilder.DropForeignKey(
                name: "FK_Question_MediaFile_FkFileId",
                table: "Question");

            migrationBuilder.DropForeignKey(
                name: "FK_QuizItem_AspNetUsers_FkUserId",
                table: "QuizItem");

            migrationBuilder.DropForeignKey(
                name: "FK_QuizItem_Question_QuestionsQuestionId",
                table: "QuizItem");

            migrationBuilder.DropPrimaryKey(
                name: "PK_QuizItem",
                table: "QuizItem");

            migrationBuilder.DropIndex(
                name: "IX_QuizItem_QuestionsQuestionId",
                table: "QuizItem");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Question",
                table: "Question");

            migrationBuilder.DropPrimaryKey(
                name: "PK_MediaType",
                table: "MediaType");

            migrationBuilder.DropPrimaryKey(
                name: "PK_MediaFile",
                table: "MediaFile");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Answer",
                table: "Answer");

            migrationBuilder.DropColumn(
                name: "QuestionsQuestionId",
                table: "QuizItem");

            migrationBuilder.RenameTable(
                name: "QuizItem",
                newName: "QuizItems");

            migrationBuilder.RenameTable(
                name: "Question",
                newName: "Questions");

            migrationBuilder.RenameTable(
                name: "MediaType",
                newName: "MediaTypes");

            migrationBuilder.RenameTable(
                name: "MediaFile",
                newName: "MediaFiles");

            migrationBuilder.RenameTable(
                name: "Answer",
                newName: "Answers");

            migrationBuilder.RenameIndex(
                name: "IX_QuizItem_FkUserId",
                table: "QuizItems",
                newName: "IX_QuizItems_FkUserId");

            migrationBuilder.RenameIndex(
                name: "IX_Question_FkUserId",
                table: "Questions",
                newName: "IX_Questions_FkUserId");

            migrationBuilder.RenameIndex(
                name: "IX_Question_FkFileId",
                table: "Questions",
                newName: "IX_Questions_FkFileId");

            migrationBuilder.RenameIndex(
                name: "IX_MediaFile_FkMediaTypeId",
                table: "MediaFiles",
                newName: "IX_MediaFiles_FkMediaTypeId");

            migrationBuilder.RenameIndex(
                name: "IX_Answer_FkQuestionId",
                table: "Answers",
                newName: "IX_Answers_FkQuestionId");

            migrationBuilder.AlterColumn<string>(
                name: "MediaFileName",
                table: "MediaFiles",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddPrimaryKey(
                name: "PK_QuizItems",
                table: "QuizItems",
                column: "QuizItemId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Questions",
                table: "Questions",
                column: "QuestionId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_MediaTypes",
                table: "MediaTypes",
                column: "MediaId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_MediaFiles",
                table: "MediaFiles",
                column: "MediaFileId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Answers",
                table: "Answers",
                column: "AnswerId");

            migrationBuilder.CreateIndex(
                name: "IX_QuizItems_FkQuestionId",
                table: "QuizItems",
                column: "FkQuestionId");

            migrationBuilder.CreateIndex(
                name: "IX_MediaTypes_Mediatype",
                table: "MediaTypes",
                column: "Mediatype",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_MediaFiles_MediaFileName",
                table: "MediaFiles",
                column: "MediaFileName",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Answers_Questions_FkQuestionId",
                table: "Answers",
                column: "FkQuestionId",
                principalTable: "Questions",
                principalColumn: "QuestionId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_MediaFiles_MediaTypes_FkMediaTypeId",
                table: "MediaFiles",
                column: "FkMediaTypeId",
                principalTable: "MediaTypes",
                principalColumn: "MediaId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Questions_AspNetUsers_FkUserId",
                table: "Questions",
                column: "FkUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Questions_MediaFiles_FkFileId",
                table: "Questions",
                column: "FkFileId",
                principalTable: "MediaFiles",
                principalColumn: "MediaFileId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_QuizItems_AspNetUsers_FkUserId",
                table: "QuizItems",
                column: "FkUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_QuizItems_Questions_FkQuestionId",
                table: "QuizItems",
                column: "FkQuestionId",
                principalTable: "Questions",
                principalColumn: "QuestionId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Answers_Questions_FkQuestionId",
                table: "Answers");

            migrationBuilder.DropForeignKey(
                name: "FK_MediaFiles_MediaTypes_FkMediaTypeId",
                table: "MediaFiles");

            migrationBuilder.DropForeignKey(
                name: "FK_Questions_AspNetUsers_FkUserId",
                table: "Questions");

            migrationBuilder.DropForeignKey(
                name: "FK_Questions_MediaFiles_FkFileId",
                table: "Questions");

            migrationBuilder.DropForeignKey(
                name: "FK_QuizItems_AspNetUsers_FkUserId",
                table: "QuizItems");

            migrationBuilder.DropForeignKey(
                name: "FK_QuizItems_Questions_FkQuestionId",
                table: "QuizItems");

            migrationBuilder.DropPrimaryKey(
                name: "PK_QuizItems",
                table: "QuizItems");

            migrationBuilder.DropIndex(
                name: "IX_QuizItems_FkQuestionId",
                table: "QuizItems");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Questions",
                table: "Questions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_MediaTypes",
                table: "MediaTypes");

            migrationBuilder.DropIndex(
                name: "IX_MediaTypes_Mediatype",
                table: "MediaTypes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_MediaFiles",
                table: "MediaFiles");

            migrationBuilder.DropIndex(
                name: "IX_MediaFiles_MediaFileName",
                table: "MediaFiles");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Answers",
                table: "Answers");

            migrationBuilder.RenameTable(
                name: "QuizItems",
                newName: "QuizItem");

            migrationBuilder.RenameTable(
                name: "Questions",
                newName: "Question");

            migrationBuilder.RenameTable(
                name: "MediaTypes",
                newName: "MediaType");

            migrationBuilder.RenameTable(
                name: "MediaFiles",
                newName: "MediaFile");

            migrationBuilder.RenameTable(
                name: "Answers",
                newName: "Answer");

            migrationBuilder.RenameIndex(
                name: "IX_QuizItems_FkUserId",
                table: "QuizItem",
                newName: "IX_QuizItem_FkUserId");

            migrationBuilder.RenameIndex(
                name: "IX_Questions_FkUserId",
                table: "Question",
                newName: "IX_Question_FkUserId");

            migrationBuilder.RenameIndex(
                name: "IX_Questions_FkFileId",
                table: "Question",
                newName: "IX_Question_FkFileId");

            migrationBuilder.RenameIndex(
                name: "IX_MediaFiles_FkMediaTypeId",
                table: "MediaFile",
                newName: "IX_MediaFile_FkMediaTypeId");

            migrationBuilder.RenameIndex(
                name: "IX_Answers_FkQuestionId",
                table: "Answer",
                newName: "IX_Answer_FkQuestionId");

            migrationBuilder.AddColumn<Guid>(
                name: "QuestionsQuestionId",
                table: "QuizItem",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "MediaFileName",
                table: "MediaFile",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddPrimaryKey(
                name: "PK_QuizItem",
                table: "QuizItem",
                column: "QuizItemId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Question",
                table: "Question",
                column: "QuestionId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_MediaType",
                table: "MediaType",
                column: "MediaId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_MediaFile",
                table: "MediaFile",
                column: "MediaFileId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Answer",
                table: "Answer",
                column: "AnswerId");

            migrationBuilder.CreateIndex(
                name: "IX_QuizItem_QuestionsQuestionId",
                table: "QuizItem",
                column: "QuestionsQuestionId");

            migrationBuilder.AddForeignKey(
                name: "FK_Answer_Question_FkQuestionId",
                table: "Answer",
                column: "FkQuestionId",
                principalTable: "Question",
                principalColumn: "QuestionId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_MediaFile_MediaType_FkMediaTypeId",
                table: "MediaFile",
                column: "FkMediaTypeId",
                principalTable: "MediaType",
                principalColumn: "MediaId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Question_AspNetUsers_FkUserId",
                table: "Question",
                column: "FkUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Question_MediaFile_FkFileId",
                table: "Question",
                column: "FkFileId",
                principalTable: "MediaFile",
                principalColumn: "MediaFileId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_QuizItem_AspNetUsers_FkUserId",
                table: "QuizItem",
                column: "FkUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_QuizItem_Question_QuestionsQuestionId",
                table: "QuizItem",
                column: "QuestionsQuestionId",
                principalTable: "Question",
                principalColumn: "QuestionId");
        }
    }
}

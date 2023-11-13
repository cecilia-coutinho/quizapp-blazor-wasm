using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace BlazorQuizWASM.Server.Data.Migrations
{
    /// <inheritdoc />
    public partial class DesignQuestionandQuizItemÙpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "MediaTypes",
                keyColumn: "MediaId",
                keyValue: new Guid("363d5e07-e18e-4c8f-9103-49e71ce0296d"));

            migrationBuilder.DeleteData(
                table: "MediaTypes",
                keyColumn: "MediaId",
                keyValue: new Guid("b011b874-5d31-40d7-b39b-26b0d36f91bd"));

            migrationBuilder.DropColumn(
                name: "Description",
                table: "Questions");

            migrationBuilder.RenameColumn(
                name: "TimeLimit",
                table: "QuizItems",
                newName: "TimeSpent");

            migrationBuilder.AddColumn<string>(
                name: "QuestionPath",
                table: "Questions",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "TimeLimit",
                table: "Questions",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.InsertData(
                table: "MediaTypes",
                columns: new[] { "MediaId", "Mediatype" },
                values: new object[,]
                {
                    { new Guid("2a7723a4-6c2b-4ce8-ac6b-ab3b924e6aeb"), "video" },
                    { new Guid("7427e04d-cef2-4c8a-8c77-238c2270927c"), "image" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Questions_QuestionPath",
                table: "Questions",
                column: "QuestionPath",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Questions_QuestionPath",
                table: "Questions");

            migrationBuilder.DeleteData(
                table: "MediaTypes",
                keyColumn: "MediaId",
                keyValue: new Guid("2a7723a4-6c2b-4ce8-ac6b-ab3b924e6aeb"));

            migrationBuilder.DeleteData(
                table: "MediaTypes",
                keyColumn: "MediaId",
                keyValue: new Guid("7427e04d-cef2-4c8a-8c77-238c2270927c"));

            migrationBuilder.DropColumn(
                name: "QuestionPath",
                table: "Questions");

            migrationBuilder.DropColumn(
                name: "TimeLimit",
                table: "Questions");

            migrationBuilder.RenameColumn(
                name: "TimeSpent",
                table: "QuizItems",
                newName: "TimeLimit");

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Questions",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.InsertData(
                table: "MediaTypes",
                columns: new[] { "MediaId", "Mediatype" },
                values: new object[,]
                {
                    { new Guid("363d5e07-e18e-4c8f-9103-49e71ce0296d"), "video" },
                    { new Guid("b011b874-5d31-40d7-b39b-26b0d36f91bd"), "image" }
                });
        }
    }
}

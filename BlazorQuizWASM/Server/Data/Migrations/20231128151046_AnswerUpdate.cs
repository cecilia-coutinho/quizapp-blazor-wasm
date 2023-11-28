using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace BlazorQuizWASM.Server.Data.Migrations
{
    /// <inheritdoc />
    public partial class AnswerUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "MediaTypes",
                keyColumn: "MediaId",
                keyValue: new Guid("935ca202-8458-41b9-ae85-3484774eb911"));

            migrationBuilder.DeleteData(
                table: "MediaTypes",
                keyColumn: "MediaId",
                keyValue: new Guid("da449e2d-fc7b-4886-90a2-fc8b32da0b3f"));

            migrationBuilder.AlterColumn<string>(
                name: "Content",
                table: "Answers",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.InsertData(
                table: "MediaTypes",
                columns: new[] { "MediaId", "Mediatype" },
                values: new object[,]
                {
                    { new Guid("38b16dd9-ec97-48b4-bab8-f6b76093056c"), "image" },
                    { new Guid("f05f8120-8256-453d-b15d-67cbf83cbee7"), "video" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Answers_Content",
                table: "Answers",
                column: "Content",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Answers_Content",
                table: "Answers");

            migrationBuilder.DeleteData(
                table: "MediaTypes",
                keyColumn: "MediaId",
                keyValue: new Guid("38b16dd9-ec97-48b4-bab8-f6b76093056c"));

            migrationBuilder.DeleteData(
                table: "MediaTypes",
                keyColumn: "MediaId",
                keyValue: new Guid("f05f8120-8256-453d-b15d-67cbf83cbee7"));

            migrationBuilder.AlterColumn<string>(
                name: "Content",
                table: "Answers",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.InsertData(
                table: "MediaTypes",
                columns: new[] { "MediaId", "Mediatype" },
                values: new object[,]
                {
                    { new Guid("935ca202-8458-41b9-ae85-3484774eb911"), "image" },
                    { new Guid("da449e2d-fc7b-4886-90a2-fc8b32da0b3f"), "video" }
                });
        }
    }
}

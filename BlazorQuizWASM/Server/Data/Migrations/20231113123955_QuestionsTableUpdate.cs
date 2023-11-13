using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace BlazorQuizWASM.Server.Data.Migrations
{
    /// <inheritdoc />
    public partial class QuestionsTableUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "MediaTypes",
                keyColumn: "MediaId",
                keyValue: new Guid("2a7723a4-6c2b-4ce8-ac6b-ab3b924e6aeb"));

            migrationBuilder.DeleteData(
                table: "MediaTypes",
                keyColumn: "MediaId",
                keyValue: new Guid("7427e04d-cef2-4c8a-8c77-238c2270927c"));

            migrationBuilder.AddColumn<bool>(
                name: "IsPublished",
                table: "Questions",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.InsertData(
                table: "MediaTypes",
                columns: new[] { "MediaId", "Mediatype" },
                values: new object[,]
                {
                    { new Guid("935ca202-8458-41b9-ae85-3484774eb911"), "image" },
                    { new Guid("da449e2d-fc7b-4886-90a2-fc8b32da0b3f"), "video" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "MediaTypes",
                keyColumn: "MediaId",
                keyValue: new Guid("935ca202-8458-41b9-ae85-3484774eb911"));

            migrationBuilder.DeleteData(
                table: "MediaTypes",
                keyColumn: "MediaId",
                keyValue: new Guid("da449e2d-fc7b-4886-90a2-fc8b32da0b3f"));

            migrationBuilder.DropColumn(
                name: "IsPublished",
                table: "Questions");

            migrationBuilder.InsertData(
                table: "MediaTypes",
                columns: new[] { "MediaId", "Mediatype" },
                values: new object[,]
                {
                    { new Guid("2a7723a4-6c2b-4ce8-ac6b-ab3b924e6aeb"), "video" },
                    { new Guid("7427e04d-cef2-4c8a-8c77-238c2270927c"), "image" }
                });
        }
    }
}

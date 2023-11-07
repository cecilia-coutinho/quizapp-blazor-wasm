using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace BlazorQuizWASM.Server.Data.Migrations
{
    /// <inheritdoc />
    public partial class Update2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "MediaTypes",
                keyColumn: "MediaId",
                keyValue: new Guid("06c5ae96-6e46-4521-be81-674c8e8b791b"));

            migrationBuilder.DeleteData(
                table: "MediaTypes",
                keyColumn: "MediaId",
                keyValue: new Guid("e14ef316-a81d-4904-954e-53f4b579f0a2"));

            migrationBuilder.InsertData(
                table: "MediaTypes",
                columns: new[] { "MediaId", "Mediatype" },
                values: new object[,]
                {
                    { new Guid("363d5e07-e18e-4c8f-9103-49e71ce0296d"), "video" },
                    { new Guid("b011b874-5d31-40d7-b39b-26b0d36f91bd"), "image" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "MediaTypes",
                keyColumn: "MediaId",
                keyValue: new Guid("363d5e07-e18e-4c8f-9103-49e71ce0296d"));

            migrationBuilder.DeleteData(
                table: "MediaTypes",
                keyColumn: "MediaId",
                keyValue: new Guid("b011b874-5d31-40d7-b39b-26b0d36f91bd"));

            migrationBuilder.InsertData(
                table: "MediaTypes",
                columns: new[] { "MediaId", "Mediatype" },
                values: new object[,]
                {
                    { new Guid("06c5ae96-6e46-4521-be81-674c8e8b791b"), "video" },
                    { new Guid("e14ef316-a81d-4904-954e-53f4b579f0a2"), "image" }
                });
        }
    }
}

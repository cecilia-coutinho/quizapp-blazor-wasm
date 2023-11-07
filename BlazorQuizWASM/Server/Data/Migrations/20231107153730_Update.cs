using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace BlazorQuizWASM.Server.Data.Migrations
{
    /// <inheritdoc />
    public partial class Update : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "MediaTypes",
                keyColumn: "MediaId",
                keyValue: new Guid("1b9aa505-9c0d-4601-8563-73b820bc6c45"));

            migrationBuilder.DeleteData(
                table: "MediaTypes",
                keyColumn: "MediaId",
                keyValue: new Guid("8445a089-8109-4d5d-9ddf-026e166a3866"));

            migrationBuilder.InsertData(
                table: "MediaTypes",
                columns: new[] { "MediaId", "Mediatype" },
                values: new object[,]
                {
                    { new Guid("4002211f-edc7-4384-84ef-3a1ff770e57d"), "video" },
                    { new Guid("b435a1ff-0cd6-4b30-b365-df126c22e842"), "image" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "MediaTypes",
                keyColumn: "MediaId",
                keyValue: new Guid("4002211f-edc7-4384-84ef-3a1ff770e57d"));

            migrationBuilder.DeleteData(
                table: "MediaTypes",
                keyColumn: "MediaId",
                keyValue: new Guid("b435a1ff-0cd6-4b30-b365-df126c22e842"));

            migrationBuilder.InsertData(
                table: "MediaTypes",
                columns: new[] { "MediaId", "Mediatype" },
                values: new object[,]
                {
                    { new Guid("1b9aa505-9c0d-4601-8563-73b820bc6c45"), "image" },
                    { new Guid("8445a089-8109-4d5d-9ddf-026e166a3866"), "video" }
                });
        }
    }
}

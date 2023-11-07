using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace BlazorQuizWASM.Server.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdateDatabase : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
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
                    { new Guid("06c5ae96-6e46-4521-be81-674c8e8b791b"), "video" },
                    { new Guid("e14ef316-a81d-4904-954e-53f4b579f0a2"), "image" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
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
                    { new Guid("4002211f-edc7-4384-84ef-3a1ff770e57d"), "video" },
                    { new Guid("b435a1ff-0cd6-4b30-b365-df126c22e842"), "image" }
                });
        }
    }
}

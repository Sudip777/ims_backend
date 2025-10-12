using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace inventory_management_system.Migrations
{
    /// <inheritdoc />
    public partial class SeedMethods : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Methods",
                columns: new[] { "MethodId", "MethodName" },
                values: new object[,]
                {
                    { 1, "GET" },
                    { 2, "POST" },
                    { 3, "PUT" },
                    { 4, "DELETE" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Methods",
                keyColumn: "MethodId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Methods",
                keyColumn: "MethodId",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Methods",
                keyColumn: "MethodId",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Methods",
                keyColumn: "MethodId",
                keyValue: 4);
        }
    }
}

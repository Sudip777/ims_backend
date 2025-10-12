using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace inventory_management_system.Migrations
{
    /// <inheritdoc />
    public partial class PatchMethodAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Methods",
                columns: new[] { "MethodId", "MethodName" },
                values: new object[] { 5, "PATCH" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Methods",
                keyColumn: "MethodId",
                keyValue: 5);
        }
    }
}

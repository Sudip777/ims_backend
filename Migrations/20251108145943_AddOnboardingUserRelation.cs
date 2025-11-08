using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace inventory_management_system.Migrations
{
    /// <inheritdoc />
    public partial class AddOnboardingUserRelation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "OnboardingId",
                table: "Users",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Onboarding",
                columns: table => new
                {
                    OnboardinigId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BusinessName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Domain = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Identity = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StorageSize = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Onboarding", x => x.OnboardinigId);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Users_OnboardingId",
                table: "Users",
                column: "OnboardingId",
                unique: true,
                filter: "[OnboardingId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Onboarding_OnboardingId",
                table: "Users",
                column: "OnboardingId",
                principalTable: "Onboarding",
                principalColumn: "OnboardinigId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_Onboarding_OnboardingId",
                table: "Users");

            migrationBuilder.DropTable(
                name: "Onboarding");

            migrationBuilder.DropIndex(
                name: "IX_Users_OnboardingId",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "OnboardingId",
                table: "Users");
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace inventory_management_system.Migrations
{
    /// <inheritdoc />
    public partial class AddOnboardingUserRelation3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_Onboarding_OnboardingId",
                table: "Users");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Onboarding",
                table: "Onboarding");

            migrationBuilder.RenameTable(
                name: "Onboarding",
                newName: "Onboardings");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Onboardings",
                table: "Onboardings",
                column: "OnboardinigId");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Onboardings_OnboardingId",
                table: "Users",
                column: "OnboardingId",
                principalTable: "Onboardings",
                principalColumn: "OnboardinigId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_Onboardings_OnboardingId",
                table: "Users");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Onboardings",
                table: "Onboardings");

            migrationBuilder.RenameTable(
                name: "Onboardings",
                newName: "Onboarding");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Onboarding",
                table: "Onboarding",
                column: "OnboardinigId");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Onboarding_OnboardingId",
                table: "Users",
                column: "OnboardingId",
                principalTable: "Onboarding",
                principalColumn: "OnboardinigId");
        }
    }
}

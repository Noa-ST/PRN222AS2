using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FUNewsManagementSystem.Core.Migrations
{
    /// <inheritdoc />
    public partial class Fixmodel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_NewsArticles_Accounts_CreatedBy",
                table: "NewsArticles");

            migrationBuilder.DropIndex(
                name: "IX_NewsArticles_CreatedBy",
                table: "NewsArticles");

            migrationBuilder.AddColumn<int>(
                name: "AccountId",
                table: "NewsArticles",
                type: "int",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Accounts",
                keyColumn: "AccountId",
                keyValue: 1,
                column: "Password",
                value: "$2a$11$03q/8.Fq0NQg80MaraYk4OUU9XN/d..VpzO4FfJaN2qSRuZkChnw6");

            migrationBuilder.CreateIndex(
                name: "IX_NewsArticles_AccountId",
                table: "NewsArticles",
                column: "AccountId");

            migrationBuilder.AddForeignKey(
                name: "FK_NewsArticles_Accounts_AccountId",
                table: "NewsArticles",
                column: "AccountId",
                principalTable: "Accounts",
                principalColumn: "AccountId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_NewsArticles_Accounts_AccountId",
                table: "NewsArticles");

            migrationBuilder.DropIndex(
                name: "IX_NewsArticles_AccountId",
                table: "NewsArticles");

            migrationBuilder.DropColumn(
                name: "AccountId",
                table: "NewsArticles");

            migrationBuilder.UpdateData(
                table: "Accounts",
                keyColumn: "AccountId",
                keyValue: 1,
                column: "Password",
                value: "$2a$11$yv06Qh8ftpH1chUmQi.I7OdlcWpCvnkoVdKmUNky8nDqOo3PZk.T6");

            migrationBuilder.CreateIndex(
                name: "IX_NewsArticles_CreatedBy",
                table: "NewsArticles",
                column: "CreatedBy");

            migrationBuilder.AddForeignKey(
                name: "FK_NewsArticles_Accounts_CreatedBy",
                table: "NewsArticles",
                column: "CreatedBy",
                principalTable: "Accounts",
                principalColumn: "AccountId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

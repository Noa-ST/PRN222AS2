using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FUNewsManagementSystem.Core.Migrations
{
    /// <inheritdoc />
    public partial class fixcategory : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "CreatedBy",
                table: "NewsArticles",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.UpdateData(
                table: "Accounts",
                keyColumn: "AccountId",
                keyValue: 1,
                column: "Password",
                value: "$2a$11$wHrXCLZunymyVMVor47jHeESY9Je6u0GNKU7RKMVywepwMr/cEEyq");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "CreatedBy",
                table: "NewsArticles",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.UpdateData(
                table: "Accounts",
                keyColumn: "AccountId",
                keyValue: 1,
                column: "Password",
                value: "$2a$11$FMdk.k1NP94yiEd/gcmHGuPkFsVbjrAPTYM98bDjU1OrcJYP5PUq2");
        }
    }
}

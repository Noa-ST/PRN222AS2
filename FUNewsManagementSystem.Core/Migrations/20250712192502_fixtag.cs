using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FUNewsManagementSystem.Core.Migrations
{
    /// <inheritdoc />
    public partial class fixtag : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Accounts",
                keyColumn: "AccountId",
                keyValue: 1,
                column: "Password",
                value: "$2a$11$vBVo6ZOKju8mX6IOV3eWYeUR5p7bfNaoga9PIwyq3U08cf1hFW2Vu");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Accounts",
                keyColumn: "AccountId",
                keyValue: 1,
                column: "Password",
                value: "$2a$11$wHrXCLZunymyVMVor47jHeESY9Je6u0GNKU7RKMVywepwMr/cEEyq");
        }
    }
}

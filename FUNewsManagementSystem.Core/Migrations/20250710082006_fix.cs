using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FUNewsManagementSystem.Core.Migrations
{
    /// <inheritdoc />
    public partial class fix : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Accounts",
                keyColumn: "AccountId",
                keyValue: 1,
                column: "Password",
                value: "$2a$11$USbU2dmAAlb.2Ua1R2HZqO/9QrrBRI4PbvccimEwaYZ2CY1RIGBMK");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Accounts",
                keyColumn: "AccountId",
                keyValue: 1,
                column: "Password",
                value: "$2a$11$03q/8.Fq0NQg80MaraYk4OUU9XN/d..VpzO4FfJaN2qSRuZkChnw6");
        }
    }
}

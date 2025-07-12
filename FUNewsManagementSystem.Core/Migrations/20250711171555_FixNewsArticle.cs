using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FUNewsManagementSystem.Core.Migrations
{
    /// <inheritdoc />
    public partial class FixNewsArticle : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ModifiedBy",
                table: "NewsArticles",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ModifiedDate",
                table: "NewsArticles",
                type: "datetime2",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Accounts",
                keyColumn: "AccountId",
                keyValue: 1,
                column: "Password",
                value: "$2a$11$FMdk.k1NP94yiEd/gcmHGuPkFsVbjrAPTYM98bDjU1OrcJYP5PUq2");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ModifiedBy",
                table: "NewsArticles");

            migrationBuilder.DropColumn(
                name: "ModifiedDate",
                table: "NewsArticles");

            migrationBuilder.UpdateData(
                table: "Accounts",
                keyColumn: "AccountId",
                keyValue: 1,
                column: "Password",
                value: "$2a$11$USbU2dmAAlb.2Ua1R2HZqO/9QrrBRI4PbvccimEwaYZ2CY1RIGBMK");
        }
    }
}

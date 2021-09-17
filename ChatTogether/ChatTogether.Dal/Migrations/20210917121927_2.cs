using Microsoft.EntityFrameworkCore.Migrations;

namespace ChatTogether.Dal.Migrations
{
    public partial class _2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Accounts_BlockedAccounts_BlockedAccountId",
                table: "Accounts");

            migrationBuilder.AddForeignKey(
                name: "FK_Accounts_BlockedAccounts_BlockedAccountId",
                table: "Accounts",
                column: "BlockedAccountId",
                principalTable: "BlockedAccounts",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Accounts_BlockedAccounts_BlockedAccountId",
                table: "Accounts");

            migrationBuilder.AddForeignKey(
                name: "FK_Accounts_BlockedAccounts_BlockedAccountId",
                table: "Accounts",
                column: "BlockedAccountId",
                principalTable: "BlockedAccounts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

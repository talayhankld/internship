using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GitTransactionsService.Migrations
{
    /// <inheritdoc />
    public partial class CancelTransactionRequest : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CancelAmount",
                table: "Transactions",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "CurrentAmount",
                table: "Transactions",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CancelAmount",
                table: "Transactions");

            migrationBuilder.DropColumn(
                name: "CurrentAmount",
                table: "Transactions");
        }
    }
}

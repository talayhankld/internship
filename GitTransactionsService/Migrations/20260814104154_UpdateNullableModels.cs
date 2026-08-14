using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GitTransactionsService.Migrations
{
    /// <inheritdoc />
    public partial class UpdateNullableModels : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "TerminalId",
                table: "Transactions",
                newName: "TerminalNo");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "TerminalNo",
                table: "Transactions",
                newName: "TerminalId");
        }
    }
}

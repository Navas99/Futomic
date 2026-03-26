using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Futomic.Migrations
{
    /// <inheritdoc />
    public partial class EditMatch : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Date",
                table: "Matches",
                newName: "PlayedAt");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PlayedAt",
                table: "Matches",
                newName: "Date");
        }
    }
}

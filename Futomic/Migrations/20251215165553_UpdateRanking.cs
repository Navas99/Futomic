using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Futomic.Migrations
{
    /// <inheritdoc />
    public partial class UpdateRanking : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UltimoResultado",
                table: "Rankings");

            migrationBuilder.RenameColumn(
                name: "Puntos",
                table: "Rankings",
                newName: "Points");

            migrationBuilder.AddColumn<string>(
                name: "LastMatchResult",
                table: "Rankings",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LastMatchResult",
                table: "Rankings");

            migrationBuilder.RenameColumn(
                name: "Points",
                table: "Rankings",
                newName: "Puntos");

            migrationBuilder.AddColumn<int>(
                name: "UltimoResultado",
                table: "Rankings",
                type: "int",
                nullable: true);
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Futomic.Migrations
{
    /// <inheritdoc />
    public partial class DeleteModelLevel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Teams_Levels_LevelId",
                table: "Teams");

            migrationBuilder.DropTable(
                name: "Levels");

            migrationBuilder.DropIndex(
                name: "IX_Teams_LevelId",
                table: "Teams");

            migrationBuilder.RenameColumn(
                name: "LevelId",
                table: "Teams",
                newName: "Level");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Level",
                table: "Teams",
                newName: "LevelId");

            migrationBuilder.CreateTable(
                name: "Levels",
                columns: table => new
                {
                    LevelId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LevelTeam = table.Column<int>(type: "int", nullable: false),
                    MaxPoints = table.Column<int>(type: "int", nullable: false),
                    MinPoints = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Levels", x => x.LevelId);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Teams_LevelId",
                table: "Teams",
                column: "LevelId");

            migrationBuilder.AddForeignKey(
                name: "FK_Teams_Levels_LevelId",
                table: "Teams",
                column: "LevelId",
                principalTable: "Levels",
                principalColumn: "LevelId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}

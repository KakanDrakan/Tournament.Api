using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Tournament.Data.Migrations
{
    /// <inheritdoc />
    public partial class renamedtournamenttable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Game_TournamentDetails_TournamentId",
                table: "Game");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TournamentDetails",
                table: "TournamentDetails");

            migrationBuilder.RenameTable(
                name: "TournamentDetails",
                newName: "Tournament");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Tournament",
                table: "Tournament",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Game_Tournament_TournamentId",
                table: "Game",
                column: "TournamentId",
                principalTable: "Tournament",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Game_Tournament_TournamentId",
                table: "Game");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Tournament",
                table: "Tournament");

            migrationBuilder.RenameTable(
                name: "Tournament",
                newName: "TournamentDetails");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TournamentDetails",
                table: "TournamentDetails",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Game_TournamentDetails_TournamentId",
                table: "Game",
                column: "TournamentId",
                principalTable: "TournamentDetails",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
